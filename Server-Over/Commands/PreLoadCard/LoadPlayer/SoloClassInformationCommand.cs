using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Processor.Class.Effect;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

public class SoloClassInformationCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;
    private readonly ClassEffectDeterminer _classEffectDeterminer = new ();

    public SoloClassInformationCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var soloClassInformation = _context.SoloClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        loadPlayer.ClassIdSolo = soloClassInformation.ClassId;
        loadPlayer.ClassChangeStatusSolo = soloClassInformation.ClassChangeStatus;

        loadPlayer.TopPointRankNumSolo = soloClassInformation.TopPointRankEntryCount;
        loadPlayer.RateSolo = soloClassInformation.Rate;

        if (soloClassInformation.ClassId < 4)
        {
            return;
        }
        
        var soloRank = _context.SoloOverRankViews
            .FirstOrDefault(rank => rank.CardId == cardProfile.Id && rank.Rank <= 1000);

        if (soloRank is not null)
        {
            loadPlayer.TopPointRankSolo = soloRank.Rank;
            _classEffectDeterminer.Determine(4, soloRank.Rank);
        }
    }
}