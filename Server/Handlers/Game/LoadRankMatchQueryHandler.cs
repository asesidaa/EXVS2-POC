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
                RankMatchSeasons =
                {
                    new Response.LoadRankMatch.RankMatchSeasonInfo
                    {
                        SeasonId = 1,
                        StartDate = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds(),
                        EndDate = (ulong)(DateTimeOffset.Now + TimeSpan.FromDays(365)).ToUnixTimeSeconds(),
                        OffEndDate  = (ulong)(DateTimeOffset.Now + TimeSpan.FromDays(366)).ToUnixTimeSeconds(),
                    }
                },
                RankMatchActives =
                {
                    new Response.LoadRankMatch.RankMatchActive
                    {
                        TeamType = 1,
                        RankZone = 1,
                        MatchingRankZone = 1
                    }   
                },
                RankBaseWinPoint = 20,
                RankBaseLosePoint = 1,
                RankLoseResultBonus = 1,
                RankMatchNumDays = 360,
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