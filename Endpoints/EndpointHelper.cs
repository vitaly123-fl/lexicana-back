using System.Net;

namespace lexicana.Endpoints;

public static class EndpointHelper
{
    public static IResult ApiResponse<T>(Response<T> response) =>
        response switch
        {
            { StatusCode: HttpStatusCode.OK, Value: FileNameResponse fileResponse } =>
                Results.File(fileResponse.Url, fileResponse.ContentType, fileResponse.Name),

            { StatusCode: HttpStatusCode.OK, Value: var value } =>
                Results.Ok(value),

            { StatusCode: HttpStatusCode.Created, Value: var value } =>
                Results.Created("", value),

            { StatusCode: HttpStatusCode.NoContent } =>
                Results.NoContent(),

            { StatusCode: HttpStatusCode.NotFound } =>
                Results.NotFound(response),

            { StatusCode: HttpStatusCode.BadRequest } =>
                Results.BadRequest(response),

            { StatusCode: HttpStatusCode.Unauthorized } =>
                Results.Unauthorized(),

            _ => Results.StatusCode((int)response.StatusCode)
        };
}