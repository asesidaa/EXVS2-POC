using nue.protocol.exvs;
using ServerOver.Mapper.Card.Setting;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadGamepadCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public LoadGamepadCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var gamepadSetting = _context.GamepadSettingDbSet
            .First(x => x.CardProfile == cardProfile);
        
        mobileUserGroup.Gamepad = gamepadSetting.ToGamepadGroup();
    }
}