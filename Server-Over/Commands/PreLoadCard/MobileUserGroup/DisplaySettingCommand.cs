using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class DisplaySettingCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public DisplaySettingCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.OpenRecord = playerProfile.OpenRecord;
        mobileUserGroup.OpenEchelon = playerProfile.OpenEchelon;
        mobileUserGroup.OpenSkillpoint = playerProfile.OpenSkillpoint;
    }
}