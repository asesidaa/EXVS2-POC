using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class RadarSettingCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public RadarSettingCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var generalSetting = _context.GeneralSettingDbSet
            .First(x => x.CardProfile == cardProfile);
        
        mobileUserGroup.RadarType = generalSetting.FixPositionRadar;
    }
}