using lexicana.Endpoints;

namespace lexicana.UserFolder.UserLessonFolder.Command.AddCompleteWord;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<AddCompleteWordRequest, EmptyValue>("users/lessons/{id}/words")
            .WithTags(EndpointTagEnum.User);
    }
}