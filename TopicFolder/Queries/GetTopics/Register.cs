using lexicana.Endpoints;

namespace lexicana.TopicFolder.Queries.GetTopics;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetTopicsRequest, List<TopicsResponseBody>>("topics")
            .WithTags(EndpointTagEnum.Topic);
    }
}