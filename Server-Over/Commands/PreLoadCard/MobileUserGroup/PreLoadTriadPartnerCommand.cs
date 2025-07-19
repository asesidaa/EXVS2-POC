using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class PreLoadTriadPartnerCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public PreLoadTriadPartnerCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var triadPartner = _context.TriadPartnerDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.customize_group.MstMobileSuitId = triadPartner.MstMobileSuitId;
        mobileUserGroup.customize_group.MsSkill1 = triadPartner.MstMobileSuitId > 0 ? triadPartner.MsSkill1 : 0;
        mobileUserGroup.customize_group.MsSkill2 = triadPartner.MstMobileSuitId > 0 ? triadPartner.MsSkill2 : 0;
    }
}