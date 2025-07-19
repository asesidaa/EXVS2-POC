using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class PreLoadMobileUserBasicInformationCommand : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context;

    public PreLoadMobileUserBasicInformationCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        mobileUserGroup.UserId = (uint)cardProfile.Id;
        mobileUserGroup.PlayerName = cardProfile.UserName;
        mobileUserGroup.KeyconfigNumber = 1;
        mobileUserGroup.Gp = cardProfile.Gp;
    }
}