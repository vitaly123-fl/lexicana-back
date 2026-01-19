using lexicana.Endpoints;

namespace lexicana.UserFolder.UserLessonFolder.Queries.GetCompletedWords;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetCompletedWordsRequest, List<Guid>>("users/lessons/{id}/words")
            .WithTags(EndpointTagEnum.User);
    }
}