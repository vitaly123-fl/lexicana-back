using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.UpdatePassword;

public class Register : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePut<UpdateUserPasswordRequest, EmptyValue>("users/password")
            .WithTags(EndpointTagEnum.User);
    }
}