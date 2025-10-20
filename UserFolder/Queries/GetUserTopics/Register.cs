using lexicana.Endpoints;

namespace lexicana.UserFolder.Queries.GetUserTopics;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetUserTopicsRequest, List<UserTopicsResponseBody>>("users/topics")
            .WithTags(EndpointTagEnum.User);
    }
}