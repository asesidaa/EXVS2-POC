using MediatR;
using nue.protocol.exvs;

namespace ServerOver.Handlers.Game;

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
                Url = "https://w.atwiki.jp/exvs2ob",
                Qrcode = "https://w.atwiki.jp/exvs2ob"
            }
        });
    }
}
