using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using Server.Models.Config;
using Server.Strategy;

namespace Server.Handlers.Game;

public record LoadGameDataQuery(Request Request) : IRequest<Response>;

public class LoadGameDataQueryHandler : IRequestHandler<LoadGameDataQuery, Response>
{
    private readonly CardServerConfig config;

    public LoadGameDataQueryHandler(IOptions<CardServerConfig> options)
    {
        config = options.Value;
    }
    
    public Task<Response> Handle(LoadGameDataQuery query, CancellationToken cancellationToken)
    {
        var allMsIds = Enumerable.Range(1, 400).Select(i => (uint)i).ToArray();

        var availableEchelonTables = Enumerable.Range(1, 55)
            .Select(i => new Response.LoadGameData.EchelonTable
            {
                EchelonId = (uint)i,
                UpDefaultExp = 0,
                DownDefaultExp = 0
            })
            .ToList();
        
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
                ReleaseGameRules = new[] { 0u, 1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u, 10u, 11u, 12u, 13u, 14u },
                UpdateMsIds = Array.Empty<uint>(), // add a 'update' tag to ms
                on_vs_info = new Response.LoadGameData.OnVsInfo
                {
                    RuleTimeLimitRank = config.GameConfigurations.BattleSeconds,
                    RuleTimeLimitCasual = config.GameConfigurations.BattleSeconds,
                    RuleTimeLimitEx = config.GameConfigurations.BattleSeconds,
                    RuleDamageLevelTeam = config.GameConfigurations.PvPDamageLevel,
                    RuleDamageLevelShuffle = config.GameConfigurations.PvPDamageLevel
                },
                ReleaseCpuScenes = new[] { 1u, 2u },
                MstMobileSuitIds = new[] { 1u, 2u }, // For Red-Targeted MSs
                OfflineWinEchelonNums = {},
                OfflineLoseEchelonNums = {},
                ReplayUnderEchelonId = 1,
                AdvancedReplayUnderEchelonId = 1,
                TrainingTimeLimit = config.GameConfigurations.TrainingMinutes,
                PcoinLoseExpRelaxationRate = 1,
                PcoinTeamGpIncreaseRate = 1,
                NewcardCampaignFlag = true,
                OfflineBaseWinPoint = config.GameConfigurations.OfflineBaseWinPoint,
                OfflineBaseLosePoint = config.GameConfigurations.OfflineBaseLosePoint,
                OfflineLoseResultBonus = config.GameConfigurations.OfflineLoseResultBonus,
                CasualBaseWinPoint = config.GameConfigurations.CasualBaseWinPoint,
                CasualBaseLosePoint = config.GameConfigurations.CasualBaseLosePoint,
                CasualLoseResultBonus = config.GameConfigurations.CasualLoseResultBonus,
                ReplayUnderRankId = 1,
                AdvancedReplayUnderRankId = 1,
                WantedDownLevel = 0, // this will cause enemy to 1 hit down if set to 1
                WantedAttackLevel = 1,
                WantedPsAttackLevel = 1,
                WantedPsDefenceLevel = 1,
                LoadGameDataVer = 27,
                score_battle_point = new Response.LoadGameData.ScoreBattlePoint
                {
                    ScoreBattle1500Point = 1500,
                    ScoreBattle2000Point = 2500,
                    ScoreBattle2500Point = 3000,
                    ScoreBattle3000Point = 4500
                },
                attack_score_setting = new Response.LoadGameData.AttackScoreSetting
                {
                    DownScoreTimes = 1,
                    Last30CountScoreTimes = 1,
                    NoAttackDecreaseScore = 1
                }
            }
        };

        response.load_game_data.FesSetting = new XrossFestStrategy().determine();
        
        // Contributing for all available Echelons, otherwise the game will freeze when there are increment of Echelon
        response.load_game_data.EchelonTables.AddRange(availableEchelonTables);
        
        // 200 - 206 are F Course 1-7, needs Fes Setting above 
        response.load_game_data.ReleaseCpuCourses.AddRange(Enumerable.Range(1, 206).Select(i =>
            new Response.LoadGameData.ReleaseCpuCourse
            {
                CourseId = (uint)i,
                OpenedAt = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds()
            }));
        return Task.FromResult(response);
    }
}