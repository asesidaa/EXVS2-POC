using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class RuleFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.on_vs_info = new Response.LoadGameData.OnVsInfo()
        {
            RuleTimeLimitTeam = 210,
            RuleTimeLimitShuffle = 210,
            RuleDamageLevelTeam = 2,
            RuleDamageLevelShuffle = 2
        };
    }
}