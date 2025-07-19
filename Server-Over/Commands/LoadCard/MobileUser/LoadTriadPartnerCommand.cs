using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadTriadPartnerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public LoadTriadPartnerCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var triadPartner = _context.TriadPartnerDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.MstMobileSuitId = triadPartner.MstMobileSuitId;
        mobileUserGroup.ArmorLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.ArmorLevel : 0;
        mobileUserGroup.ShootAttackLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.ShootAttackLevel : 0;
        mobileUserGroup.InfightAttackLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.InfightAttackLevel : 0;
        mobileUserGroup.BoosterLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.BoosterLevel : 0;
        mobileUserGroup.ExGaugeLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.ExGaugeLevel : 0;
        mobileUserGroup.AiLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.AiLevel : 0;
        mobileUserGroup.BurstType = triadPartner.MstMobileSuitId > 0 ? triadPartner.BurstType : 0;
        mobileUserGroup.TriadTeamName = triadPartner.TriadTeamName;
        mobileUserGroup.TriadBackgroundPartsId = triadPartner.MstMobileSuitId > 0 ? triadPartner.TriadBackgroundPartsId : 0;
    }
}