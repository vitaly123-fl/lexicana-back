using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.UpdateEmail;

public class Register : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePut<UpdateUserEmailRequest, EmptyValue>("users/email").WithTags(EndpointTagEnum.User);
    }
}