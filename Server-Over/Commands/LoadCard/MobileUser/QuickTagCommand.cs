using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class QuickTagCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public QuickTagCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var teamSetting = _context.TeamSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        var quickTagUserId = teamSetting.QuickOnlineTagCardId;

        if (quickTagUserId == 0)
        {
            return;
        }

        if (quickTagUserId == cardProfile.Id)
        {
            return;
        }

        var existingTeam = _context.TagTeamDataDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.TeammateCardId == quickTagUserId);

        if (existingTeam is not null)
        {
            return;
        }
        
        var partnerProfile = _context.CardProfiles
            .FirstOrDefault(x => x.Id == quickTagUserId);

        if (partnerProfile is null)
        {
            return;
        }
        
        var existingOppositeTeam = _context.TagTeamDataDbSet
            .FirstOrDefault(x => x.CardProfile == partnerProfile && x.TeammateCardId == cardProfile.Id);

        if (existingOppositeTeam is not null)
        {
            return;
        }

        var partnerPlayerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == partnerProfile);
        
        var partnerTeamClassMatchRecord = _context.TeamClassMatchRecordDbSet
            .First(x => x.CardProfile == partnerProfile);
        
        mobileUserGroup.online_tag_info.quick_tag_partner = new Response.LoadCard.MobileUserGroup.OnlineTagInfo.QuickTagPartner
        {
            Id = (uint) partnerProfile.Id,
            Name = partnerProfile.UserName,
            PlayerLevelId = partnerPlayerLevel.PlayerLevelId,
            PrestigeId = partnerPlayerLevel.PrestigeId,
            ClassIdTeam = partnerTeamClassMatchRecord.ClassId
        };
    }
}