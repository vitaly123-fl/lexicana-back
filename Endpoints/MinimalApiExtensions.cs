using MediatR;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;

namespace lexicana.Endpoints;

public class Wrapper<TModel>
{
    public Wrapper(TModel? value)
    {
        Value = value;
    }

    public TModel? Value { get; }

    public static async ValueTask<Wrapper<TModel>?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (!context.Request.HasJsonContentType())
        {
            throw new BadHttpRequestException(
                "Request content type was not a recognized JSON content type.",
                StatusCodes.Status415UnsupportedMediaType);
        }

        using var sr = new StreamReader(context.Request.Body);
        var str = await sr.ReadToEndAsync();

        return new Wrapper<TModel>(JsonConvert.DeserializeObject<TModel>(str));
    }
}

public class GenericPatchRequest<T, TResponse>: IHttpRequest<TResponse> where T : class
{
    public long Id { get; set; }
    public JsonPatchDocument<T> Patches { get; set; }
};

public static class MinimalApiExtensions
{
    public static RouteHandlerBuilder MediateGet<TRequest, TResponse>(this RouteGroupBuilder app, string template)
        where TRequest : IHttpRequest<TResponse>
    {
        return app.MapGet(template,
            async (IMediator mediator, [AsParameters] TRequest request) =>
            await ExecuteMediatR<TRequest, TResponse>(mediator, request)).RequireAuthorization();
    }

    public static RouteHandlerBuilder MediatePost<TRequest, TResponse>(this RouteGroupBuilder app, string template)
        where TRequest : IHttpRequest<TResponse>
    {
        return app.MapPost(template,
            async (IMediator mediator, [AsParameters] TRequest request) =>
            await ExecuteMediatR<TRequest, TResponse>(mediator, request)).RequireAuthorization();
    }

    public static RouteHandlerBuilder MediatePut<TRequest, TResponse>(this RouteGroupBuilder app, string template)
        where TRequest : IHttpRequest<TResponse>
    {
        return app.MapPut(template,
            async (IMediator mediator, [AsParameters] TRequest request) =>
            await ExecuteMediatR<TRequest, TResponse>(mediator, request)).RequireAuthorization();
    }

    public static RouteHandlerBuilder MediateDelete<TRequest, TResponse>(this RouteGroupBuilder app, string template)
        where TRequest : IHttpRequest<TResponse>
    {
        return app.MapDelete(template,
            async (IMediator mediator, [AsParameters] TRequest request) =>
            await ExecuteMediatR<TRequest, TResponse>(mediator, request)).RequireAuthorization();
    }

    public static RouteHandlerBuilder MediatePatch<TRequest, TRequestBody, TResponse>(this WebApplication app, string template)
        where TRequest: GenericPatchRequest<TRequestBody,TResponse> , new()
        where TRequestBody: class
    {
        return app.MapPatch(template,
            async (IMediator mediator, long id, Wrapper<JsonPatchDocument<TRequestBody>> request) =>
            {
                var newRequest = new TRequest()
                {
                    Id = id,
                    Patches = request.Value
                };

                var response = await mediator.Send(newRequest);
                return EndpointHelper.ApiResponse(response);
            }
        );
    }

    private static async Task<IResult> ExecuteMediatR<TRequest, TResponse>(IMediator mediator, TRequest request)
        where TRequest : IHttpRequest<TResponse>
    {
        var response = await mediator.Send(request);
        return EndpointHelper.ApiResponse(response);
    }
}