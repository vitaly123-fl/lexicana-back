using lexicana.Endpoints;

namespace lexicana.Authorization.Commands.Login;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<LoginUserRequest, string>("auth/login")
            .WithTags(EndpointTagEnum.Auth).AllowAnonymous();
    }
}