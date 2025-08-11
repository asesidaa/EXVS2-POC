using MediatR;
using nue.protocol.exvs;
using ServerOver.Utils;

namespace ServerOver.Handlers.Game;

public record LoadRankingQuery(Request Request) : IRequest<Response>;

public class LoadRankingQueryHandler : IRequestHandler<LoadRankingQuery, Response>
{
    private readonly ILogger<LoadRankingQueryHandler> _logger;

    public LoadRankingQueryHandler(ILogger<LoadRankingQueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<Response> Handle(LoadRankingQuery request, CancellationToken cancellationToken)
    {
        var loadRanking = new Response.LoadRanking();

        var loadRankingRequest = request.Request.load_ranking;

        if (loadRankingRequest.RankType == RankMessageType.LmMonthlySpotPlayerScore && loadRankingRequest.CurrentFlag == true)
        {
            loadRanking.RankType = RankMessageType.LmMonthlySpotPlayerScore;

            var liveRankingTime = LiveRankingTimeUtil.Get();

            loadRanking.Timestamp = liveRankingTime.CurrentTimeStamp;
            
            var playerScoreRank = new PlayerScoreRank();
            playerScoreRank.SumStart = liveRankingTime.MonthStartTimeStamp;
            playerScoreRank.SumEnd = liveRankingTime.MonthEndTimeStamp;
            
            for (uint i = 1; i < 11; i++)
            {
                playerScoreRank.Rows.Add(new PlayerScoreRank.Row()
                {
                    RankNo = i,
                    PrevRankNo = i,
                    PilotId = i,
                    PlayerName = "Player" + i.ToString(),
                    Score = 10000 * (10 - i),
                    PlayerLevelId = 1,
                    PrestigeId = 1,
                    TitleTextId = 0,
                    TitleOrnamentId = 0,
                    TitleEffectId = 0,
                    TitleBackgroundPartsId = 0,
                    CustomTxt = "Hello",
                    FavMsId = 123,
                    MsUsedNum = 6000,
                    HomeLocName = "NEXTREME",
                    HomeLocPref = 1,
                    OpenRecord = 1,
                    PlayNum = 100,
                    WinNum = 100,
                    ClassIdSolo = 1,
                    ClassIdTeam = 1,
                    SkinId = 0
                });
            }
            
            loadRanking.PlayerScoreRank = playerScoreRank;
        }
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_ranking = loadRanking
        });
    }
}