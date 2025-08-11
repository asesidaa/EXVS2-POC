using nue.protocol.exvs;
using ServerOver.Constants.ClassMatch;

namespace ServerOver.Commands.ClassMatch;

public class OverClassChangePercentageCommand : IClassMatchFillerCommand
{
    public void Fill(Response.LoadClassMatch loadClassMatchResponse)
    {
        loadClassMatchResponse.ClassChangePercentages.Add(new Response.LoadClassMatch.ClassChangePercentage()
        {
            TeamType = ClassTeamType.SoloOnlineClass,
            ClassId = RankType.Over,
            PromotePercentage = 0,
            RelagatePercentage = 80
        });
        
        loadClassMatchResponse.ClassChangePercentages.Add(new Response.LoadClassMatch.ClassChangePercentage()
        {
            TeamType = ClassTeamType.TeamOnlineClass,
            ClassId = RankType.Over,
            PromotePercentage = 0,
            RelagatePercentage = 85
        });
    }
}