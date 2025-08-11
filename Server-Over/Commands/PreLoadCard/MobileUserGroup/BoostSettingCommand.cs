using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class BoostSettingCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public BoostSettingCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var boostSetting = _context.BoostSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.customize_group.GpBoost = boostSetting.GpBoost;
        mobileUserGroup.customize_group.GuestNavBoost = boostSetting.GuestNavBoost;
    }
}