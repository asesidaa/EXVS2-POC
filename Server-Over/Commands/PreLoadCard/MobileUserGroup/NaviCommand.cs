using nue.protocol.exvs;
using ServerOver.Mapper.Card.Navi;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class NaviCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public NaviCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var userNavis = _context.NaviDbSet
            .Where(x => x.CardProfile == cardProfile)
            .ToList();
        
        userNavis.ForEach(navi => mobileUserGroup.GuestNavs.Add(navi.ToGuestNaviGroup()));
    }
}