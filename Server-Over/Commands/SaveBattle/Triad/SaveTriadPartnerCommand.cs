using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad;

public class SaveTriadPartnerCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SaveTriadPartnerCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        if (!battleResultContext.PartnerDomain.IsTriadPartner)
        {
            return;
        }

        var partnerMsId = battleResultContext.PartnerDomain.TriadPartnerMsId;
        
        var partnerMobileSuit = _context.MobileSuitUsageDbSet
            .FirstOrDefault(x =>
                x.CardProfile == cardProfile &&
                x.MstMobileSuitId == partnerMsId
            );

        if (partnerMobileSuit is null)
        {
            _context.Add(new MobileSuitUsage()
            {
                CardProfile = cardProfile,
                MstMobileSuitId = partnerMsId,
                TriadBuddyPoint = 1
            });
            
            _context.SaveChanges();
            
            return;
        }

        partnerMobileSuit.TriadBuddyPoint += 1;
    }
}