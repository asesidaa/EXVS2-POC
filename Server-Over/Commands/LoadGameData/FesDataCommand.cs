using nue.protocol.exvs;
using ServerOver.Models.Config;
using ServerOver.Strategy;

namespace ServerOver.Commands.LoadGameData;

public class FesDataCommand : ILoadGameDataCommand
{
    private readonly CardServerConfig _config;

    public FesDataCommand(CardServerConfig config)
    {
        _config = config;
    }

    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.ReleaseGameRules = new[]
        {
            0u, 1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u, 10u, 11u, 12u, 13u, 14u,
            15u, 16u, 17u, 18u, 19u, 20u, 21u, 22u, 23u, 24u
        };
        
        loadGameData.FesSetting = new XrossFestStrategy(_config).determine();
        
        loadGameData.score_battle_point = new Response.LoadGameData.ScoreBattlePoint
        {
            ScoreBattle1500Point = 1500,
            ScoreBattle2000Point = 2500,
            ScoreBattle2500Point = 3000,
            ScoreBattle3000Point = 4500
        };
        
        loadGameData.attack_score_setting = new Response.LoadGameData.AttackScoreSetting
        {
            DownScoreTimes = 1,
            Last30CountScoreTimes = 1,
            NoAttackDecreaseScore = 10
        };
    }
}