using lexicana.Endpoints;

namespace lexicana.UserFolder.UserTopicFolder.Command.CompleteTopic;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<CompleteUserTopicRequest, EmptyValue>("users/topics/{id}").WithTags(EndpointTagEnum.User);
    }
}