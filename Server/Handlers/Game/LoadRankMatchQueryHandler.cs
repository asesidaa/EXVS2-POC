using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record LoadRankMatchQuery(Request Request) : IRequest<Response>;

public class LoadRankMatchQueryHandler : IRequestHandler<LoadRankMatchQuery, Response>
{
    public Task<Response> Handle(LoadRankMatchQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_rank_match = new Response.LoadRankMatch
            {
                RankBaseLosePoint = 1,
                RankBaseWinPoint = 1,
                RankLoseResultBonus = 1,
                RankMatchNumDays = 0,
                RankWinPointBonus1 = 1,
                RankWinPointBonus2 = 1,
                RankWinPointBonus3 = 1,
                RankLosePointBonus1 = 1,
                RankLosePointBonus2 = 1,
                RankLosePointBonus3 = 1,
                ExxThresholdOrder = 1,
                ExxThresholdPoint = 1
            }
        };
        return Task.FromResult(response);
    }
}