using lexicana.Endpoints;

namespace lexicana.Authorization.Queries.GetFirebaseToken;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetFirebaseTokenRequest, string>("auth/firebase").WithTags(EndpointTagEnum.Auth).AllowAnonymous();
    }
}