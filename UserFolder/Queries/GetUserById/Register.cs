using lexicana.Endpoints;

namespace lexicana.UserFolder.Queries.GetUserById;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetUserRequest, GetUserResponse>("users").WithTags(EndpointTagEnum.User);
    }
}