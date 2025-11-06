using lexicana.Endpoints;

namespace lexicana.ParserFolder.Commands.TopicWordParser;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePut<WordParserCommand, List<WordRecord>>("parse")
            .WithTags(EndpointTagEnum.Parse)
            .AllowAnonymous()
            .DisableAntiforgery();
    }
}