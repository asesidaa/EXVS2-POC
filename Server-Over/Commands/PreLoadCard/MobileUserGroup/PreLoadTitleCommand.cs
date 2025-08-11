using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerOver.Mapper.Card.Titles.User;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class PreLoadTitleCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public PreLoadTitleCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var userTitleSetting = _context.UserTitleSettingDbSet
            .Include(x => x.UserDefaultTitle)
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.customize_group.DefaultTitleCustomize = userTitleSetting.UserDefaultTitle.ToTitleCustomize();
        mobileUserGroup.customize_group.RandomTitleFlag = userTitleSetting.RandomTitleFlag;
    }
}