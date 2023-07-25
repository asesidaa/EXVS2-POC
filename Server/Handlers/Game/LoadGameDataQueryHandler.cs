using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record LoadGameDataQuery(Request Request) : IRequest<Response>;

public class LoadGameDataQueryHandler : IRequestHandler<LoadGameDataQuery, Response>
{
    public Task<Response> Handle(LoadGameDataQuery query, CancellationToken cancellationToken)
    {
        var allMsIds = Enumerable.Range(1, 400).Select(i => (uint)i).ToArray();
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_game_data = new Response.LoadGameData
            {
                ReleaseMsIds = allMsIds, // ms unlock ids
                NewMsIds = Array.Empty<uint>(), // responsible for showing ms under "new" series
                DisplayableMsIds = allMsIds, // responsible for triad battle ai enemy units
                ReleaseGuestNavIds = allMsIds, // responsible for triad battle ai partners
                ReleaseGameRules = new[] { 1u, 2u },
                UpdateMsIds = Array.Empty<uint>(), // add a 'update' tag to ms
                on_vs_info = new Response.LoadGameData.OnVsInfo
                {
                    RuleTimeLimitRank = 230, // player match time in seconds
                    RuleTimeLimitCasual = 230,
                    RuleTimeLimitEx = 230,
                    RuleDamageLevelTeam = 1,
                    RuleDamageLevelShuffle = 1
                },
                EchelonTables =
                {
                    new Response.LoadGameData.EchelonTable
                    {
                        EchelonId = 1
                    }
                },
                ReleaseCpuScenes = new[] { 1u, 2u },
                MstMobileSuitIds = new[] { 1u, 2u },
                OfflineWinEchelonNums =
                {
                    new Response.LoadGameData.OfflineEchelon
                    {
                        Id = 1u,
                        LowerThreshold = 1,
                        UpperThreshold = 100,
                        Point = 1
                    }
                },
                OfflineLoseEchelonNums =
                {
                    new Response.LoadGameData.OfflineEchelon
                    {
                        Id = 1,
                        LowerThreshold = 1,
                        UpperThreshold = 100,
                        Point = 1
                    }
                },

                ReplayUnderEchelonId = 1,
                AdvancedReplayUnderEchelonId = 1,
                TrainingTimeLimit = 12, // training mode's countdown time in minutes
                PcoinLoseExpRelaxationRate = 1,
                PcoinTeamGpIncreaseRate = 1,
                NewcardCampaignFlag = true,
                CasualBaseWinPoint = 1,
                CasualBaseLosePoint = 1,
                OfflineBaseLosePoint = 1,
                OfflineBaseWinPoint = 1,
                CasualLoseResultBonus = 1,
                OfflineLoseResultBonus = 1,
                ReplayUnderRankId = 1,
                AdvancedReplayUnderRankId = 1,
                WantedDownLevel = 0, // this will cause enemy to 1 hit down if set to 1
                WantedAttackLevel = 1,
                WantedPsAttackLevel = 1,
                WantedPsDefenceLevel = 1,
                LoadGameDataVer = 1,
                score_battle_point = new Response.LoadGameData.ScoreBattlePoint
                {
                    ScoreBattle1500Point = 1,
                    ScoreBattle2000Point = 1,
                    ScoreBattle2500Point = 1,
                    ScoreBattle3000Point = 1
                },
                attack_score_setting = new Response.LoadGameData.AttackScoreSetting
                {
                    DownScoreTimes = 1,
                    Last30CountScoreTimes = 1,
                    NoAttackDecreaseScore = 1
                }
            }
        };
        response.load_game_data.ReleaseCpuCourses.AddRange(Enumerable.Range(1, 100).Select(i =>
            new Response.LoadGameData.ReleaseCpuCourse
            {
                CourseId = (uint)i,
                OpenedAt = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds()
            }));
        return Task.FromResult(response);
    }
}