using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Utils;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadCustomizeGroupCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private const uint BlankStage = 0;
    private const uint RandomStage = 255;

    public LoadCustomizeGroupCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var customizeProfile = _context.CustomizeProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var stageRandoms = ArrayUtil.FromString(customizeProfile.StageRandoms)
            .ToList()
            .Where(stage => stage != BlankStage && stage != RandomStage)
            .ToArray();
        
        mobileUserGroup.Customize = new Response.LoadCard.MobileUserGroup.CustomizeGroup()
        {
            DefaultGaugeDesignId = customizeProfile.DefaultGaugeDesignId,
            DefaultBgmPlayMethod = customizeProfile.DefaultBgmPlayMethod,
            DefaultBgmSettings = ArrayUtil.FromString(customizeProfile.DefaultBgmSettings),
            StageRandoms = stageRandoms
        };
    }
}