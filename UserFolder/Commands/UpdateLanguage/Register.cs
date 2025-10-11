using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.UpdateLanguage;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePut<UpdateUserLanguageRequest, EmptyValue>("users/language").WithTags(EndpointTagEnum.User);
    }
}