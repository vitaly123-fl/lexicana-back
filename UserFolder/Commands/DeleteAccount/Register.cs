using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.DeleteAccount;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateDelete<DeleteAccountRequest, EmptyValue>("users").WithTags(EndpointTagEnum.User);
    }
}