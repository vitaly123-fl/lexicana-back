using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.UpdateName;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePut<UpdateUserNameRequest, EmptyValue>("users/name").WithTags(EndpointTagEnum.User);
    }
}