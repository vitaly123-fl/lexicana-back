using lexicana.Endpoints;

namespace lexicana.UserFolder.Queries.GetUserLessons;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetUserLessonsRequest, List<UserLessonsResponseBody>>("users/lessons")
            .WithTags(EndpointTagEnum.User);
    }
}