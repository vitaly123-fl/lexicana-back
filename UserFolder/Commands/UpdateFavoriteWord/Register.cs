using lexicana.Endpoints;

namespace lexicana.UserFolder.Commands.UpdateFavoriteWord;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePut<UpdateFavoriteWordRequest, EmptyValue>("users/favorite")
            .WithTags(EndpointTagEnum.User);
    }
}