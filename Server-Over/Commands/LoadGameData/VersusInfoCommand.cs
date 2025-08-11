using nue.protocol.exvs;

namespace ServerOver.Commands.LoadGameData;

public class VersusInfoCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.on_vs_info = new Response.LoadGameData.OnVsInfo
        {
            RuleTimeLimitClass = 230,
            RuleTimeLimitFree = 210,
            RuleDamageLevelTeam = 2,
            RuleDamageLevelShuffle = 2
        };

        loadGameData.ScoreDisplayIntervals = new[] { 10u, 20u, 30u, 60u, 120u, 180u, 240u };
    }
}