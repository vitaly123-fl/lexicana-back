using lexicana.Endpoints;

namespace lexicana.LessonFolder.Queries.GetLessons;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetLessonsRequest, List<LessonsResponseBody>>("lessons")
            .WithTags(EndpointTagEnum.Lesson);
    }
}