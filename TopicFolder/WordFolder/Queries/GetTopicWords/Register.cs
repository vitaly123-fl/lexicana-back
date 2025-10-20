using lexicana.Endpoints;
using lexicana.TopicFolder.WordFolder.DTOs;

namespace lexicana.TopicFolder.WordFolder.Queries.GetTopicWords;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetTopicWordsRequest, List<WordModel>>("topics/{id}/words")
            .WithTags(EndpointTagEnum.Topic);
    }
}