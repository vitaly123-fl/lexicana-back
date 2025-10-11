using lexicana.Endpoints;

namespace lexicana.Authorization.Commands.Registration;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<RegisterUserRequest, string>("auth/registration")
            .WithTags(EndpointTagEnum.Auth).AllowAnonymous();
    }
}