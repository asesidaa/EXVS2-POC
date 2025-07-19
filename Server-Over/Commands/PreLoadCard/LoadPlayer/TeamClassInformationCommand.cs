using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Processor.Class.Effect;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

public class TeamClassInformationCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;
    private readonly ClassEffectDeterminer _classEffectDeterminer = new ();

    public TeamClassInformationCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var teamClassInformation = _context.TeamClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        loadPlayer.ClassIdTeam = teamClassInformation.ClassId;
        loadPlayer.ClassChangeStatusTeam = teamClassInformation.ClassChangeStatus;
        
        loadPlayer.TopPointRankNumTeam = teamClassInformation.TopPointRankEntryCount;
        loadPlayer.RateTeam = teamClassInformation.Rate;
        
        if (teamClassInformation.ClassId < 4)
        {
            return;
        }
        
        var teamRank = _context.TeamOverRankViews
            .FirstOrDefault(rank => rank.CardId == cardProfile.Id && rank.Rank <= 1000);

        if (teamRank is not null)
        {
            loadPlayer.TopPointRankTeam = teamRank.Rank;
            _classEffectDeterminer.Determine(4, teamRank.Rank);
        }
    }
}