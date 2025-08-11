using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerOver.Mapper.Card.Titles.User;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadTitleCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public LoadTitleCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var userTitleSetting = _context.UserTitleSettingDbSet
            .Include(x => x.UserTriadTitle)
            .Include(x => x.UserClassMatchTitle)
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.TriadTitleCustomize = userTitleSetting.UserTriadTitle.ToTitleCustomize();
        mobileUserGroup.ClassMatchTitleCustomize = userTitleSetting.UserClassMatchTitle.ToTitleCustomize();
    }
}