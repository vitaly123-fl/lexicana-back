using lexicana.Endpoints;

namespace lexicana.TopicFolder.WordFolder.Command.SendWordReport;

public class Register: IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder app)
    {
        app.MediatePost<SendWordReportRequest, EmptyValue>("words/{id}/report")
            .WithTags(EndpointTagEnum.Word);
    }
}