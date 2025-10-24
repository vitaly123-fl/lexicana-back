using lexicana.Endpoints;
using lexicana.TopicFolder.WordFolder.DTOs;

namespace lexicana.UserFolder.Queries.GetFavoriteWords;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediateGet<GetFavoriteWordsRequest, List<WordModel>>("users/favorite")
            .WithTags(EndpointTagEnum.User);
    }
}