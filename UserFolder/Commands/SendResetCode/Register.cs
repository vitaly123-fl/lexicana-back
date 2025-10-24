using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.SendResetCode;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<SendResetCodeRequest, EmptyValue>("users/code")
            .WithTags(EndpointTagEnum.Email);
    }
}