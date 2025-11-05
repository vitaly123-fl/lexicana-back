using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.CheckExist;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<CheckExistUserRequest, EmptyValue>("users/exist")
            .WithTags(EndpointTagEnum.User).AllowAnonymous();
    }
}