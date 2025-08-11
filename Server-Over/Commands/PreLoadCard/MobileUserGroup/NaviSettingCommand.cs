using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class NaviSettingCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public NaviSettingCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var naviSetting = _context.NaviSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.customize_group.BattleNavAdviseFlag = naviSetting.BattleNavAdviseFlag;
        mobileUserGroup.customize_group.BattleNavNotifyFlag = naviSetting.BattleNavNotifyFlag;
    }
}