using lexicana.Endpoints;

namespace lexicana.UserFolder.UserLessonFolder.Command.CompleteLesson;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<CompleteUserLessonRequest, EmptyValue>("users/lessons/{id}")
            .WithTags(EndpointTagEnum.User);
    }
}