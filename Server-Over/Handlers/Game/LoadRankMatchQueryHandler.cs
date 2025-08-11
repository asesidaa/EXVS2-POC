using MediatR;
using nue.protocol.exvs;

namespace ServerOver.Handlers.Game;

public record LoadRankMatchQuery(Request Request) : IRequest<Response>;

public class LoadRankMatchQueryHandler : IRequestHandler<LoadRankMatchQuery, Response>
{
    public Task<Response> Handle(LoadRankMatchQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        
        var rankMatch = new Response.LoadRankMatch
        {
            RankMatchSeasons =
            {
                new Response.LoadRankMatch.RankMatchSeasonInfo
                {
                    SeasonId = 10,
                    StartDate = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds(),
                    EndDate = (ulong)(DateTimeOffset.Now + TimeSpan.FromDays(365)).ToUnixTimeSeconds(),
                    OffEndDate = (ulong)(DateTimeOffset.Now + TimeSpan.FromDays(366)).ToUnixTimeSeconds(),
                }
            },
            // TODO: Fill all the Thresholds from Rookie upto EXX
            RankMatchPointThresholds =
            {
                // Rookie
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 0,
                    InitNum = 0,
                    LowerPoint = 0,
                    UpperPoint = 0
                },
                // D
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 1,
                    InitNum = 10,
                    LowerPoint = 0,
                    UpperPoint = 30
                },
                // C1 - C5, Each Level Gap = 50
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 2,
                    InitNum = 40,
                    LowerPoint = 0,
                    UpperPoint = 80
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 3,
                    InitNum = 90,
                    LowerPoint = 0,
                    UpperPoint = 130
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 4,
                    InitNum = 140,
                    LowerPoint = 0,
                    UpperPoint = 180
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 5,
                    InitNum = 190,
                    LowerPoint = 0,
                    UpperPoint = 230
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 6,
                    InitNum = 240,
                    LowerPoint = 0,
                    UpperPoint = 280
                },
                // B1 - B5, Each Level Gap = 100
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 7,
                    InitNum = 290,
                    LowerPoint = 0,
                    UpperPoint = 380
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 8,
                    InitNum = 390,
                    LowerPoint = 0,
                    UpperPoint = 480
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 9,
                    InitNum = 490,
                    LowerPoint = 0,
                    UpperPoint = 580
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 10,
                    InitNum = 590,
                    LowerPoint = 0,
                    UpperPoint = 680
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 11,
                    InitNum = 690,
                    LowerPoint = 0,
                    UpperPoint = 780
                },
                // A1 - A5, Each Level Gap = 150
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 12,
                    InitNum = 800,
                    LowerPoint = 0,
                    UpperPoint = 930
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 13,
                    InitNum = 950,
                    LowerPoint = 0,
                    UpperPoint = 1080
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 14,
                    InitNum = 1100,
                    LowerPoint = 0,
                    UpperPoint = 1230
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 15,
                    InitNum = 1250,
                    LowerPoint = 0,
                    UpperPoint = 1380
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 16,
                    InitNum = 1400,
                    LowerPoint = 0,
                    UpperPoint = 1530
                },
                // S1 - S5, Each Level Gap = 200
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 17,
                    InitNum = 1550,
                    LowerPoint = 0,
                    UpperPoint = 1730
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 18,
                    InitNum = 1750,
                    LowerPoint = 0,
                    UpperPoint = 1930
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 19,
                    InitNum = 1950,
                    LowerPoint = 0,
                    UpperPoint = 2130
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 20,
                    InitNum = 2150,
                    LowerPoint = 0,
                    UpperPoint = 2330
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 21,
                    InitNum = 2350,
                    LowerPoint = 0,
                    UpperPoint = 2550
                },
                // EX and EXX
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 22,
                    InitNum = 2600,
                    LowerPoint = 0,
                    UpperPoint = 3400
                },
                new Response.LoadRankMatch.RankMatchPointThreshold
                {
                    MatchingRankId = 23,
                    InitNum = 3500,
                    LowerPoint = 0,
                    UpperPoint = int.MaxValue
                }
            },
            RankBaseWinPoint = 2,
            RankBaseLosePoint = -2,
            RankLoseResultBonus = 1,
            RankMatchNumDays = 360,
            RankWinPointBonus1 = 0,
            RankWinPointBonus2 = 0,
            RankWinPointBonus3 = 0,
            RankLosePointBonus1 = 0,
            RankLosePointBonus2 = 0,
            RankLosePointBonus3 = 0,
            ExxThresholdOrder = 30,
            ExxThresholdPoint = 3400
        };
        
        for (var level = 0; level < 56; level++)
        {
            rankMatch.RankMatchWinPointSoloes.Add(new Response.LoadRankMatch.RankMatchWinPoint()
            {
                Level = (uint) level,
                Point = 5
            });
            
            rankMatch.RankMatchWinPointTeams.Add(new Response.LoadRankMatch.RankMatchWinPoint()
            {
                Level = (uint) level,
                Point = 5
            });
        }
        
        for (uint rankZoneId = 0; rankZoneId < 24; rankZoneId++)
        {
            rankMatch.RankMatchActives.Add(new Response.LoadRankMatch.RankMatchActive
            {
                TeamType = 1,
                RankZone = rankZoneId,
                MatchingRankZone = rankZoneId
            });
            
            rankMatch.RankMatchActives.Add(new Response.LoadRankMatch.RankMatchActive
            {
                TeamType = 2,
                RankZone = rankZoneId,
                MatchingRankZone = rankZoneId
            });
            
            rankMatch.RankWinBonus.Add(new Response.LoadRankMatch.RankMatchWinBonus()
            {
                RankZoneId = rankZoneId,
                Point = 1
            });
            
            rankMatch.RankMatchLosePointSoloes.Add(new Response.LoadRankMatch.RankMatchLosePoint()
            {
                RankZoneId = rankZoneId,
                Point = rankZoneId > 2 ? -1 : 0
            });
            
            rankMatch.RankMatchLosePointTeams.Add(new Response.LoadRankMatch.RankMatchLosePoint()
            {
                RankZoneId = rankZoneId,
                Point = rankZoneId > 2 ? -1 : 0
            });
        }
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_rank_match = rankMatch
        };
        
        return Task.FromResult(response);
    }
}