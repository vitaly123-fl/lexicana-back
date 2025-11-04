using lexicana.Endpoints;

namespace lexicana.UserFolder.UserTopicFolder.Queries.GetCompletedWords;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetCompletedWordsRequest, List<Guid>>("users/topics/{id}/words")
            .WithTags(EndpointTagEnum.User);
    }
}