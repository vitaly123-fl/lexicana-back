using lexicana.Endpoints;
using lexicana.LessonFolder.TopicFolder.WordFolder.DTOs;

namespace lexicana.LessonFolder.Queries.GetLessonWords;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetLessonWordsRequest, List<WordModel>>("lessons/{id}/words")
            .WithTags(EndpointTagEnum.Lesson);
    }
}