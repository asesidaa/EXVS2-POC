using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record LoadRankingQuery(Request Request) : IRequest<Response>;

public class LoadRankingQueryHandler : IRequestHandler<LoadRankingQuery, Response>
{
    public Task<Response> Handle(LoadRankingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_ranking = new Response.LoadRanking()
        });
    }
}