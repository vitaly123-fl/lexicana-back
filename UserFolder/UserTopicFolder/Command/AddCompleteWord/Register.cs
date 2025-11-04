using lexicana.Endpoints;

namespace lexicana.UserFolder.UserTopicFolder.Command.AddCompleteWord;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<AddCompleteWordRequest, EmptyValue>("users/topics/{id}/words").WithTags(EndpointTagEnum.User);
    }
}