using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.VerifyResetCode;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<VerifyUserResetCodeRequest, EmptyValue>("users/verify")
            .WithTags(EndpointTagEnum.Email);
    }
}