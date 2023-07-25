using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record LoadSpotUrlQuery(Request Request) : IRequest<Response>;

public class LoadSpotUrlQueryHandler : IRequestHandler<LoadSpotUrlQuery, Response>
{
    public Task<Response> Handle(LoadSpotUrlQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_spot_url = new Response.LoadSpotUrl
            {
                Url = "https://example.com",
                Qrcode = "https://example.com"
            }
        });
    }
}
