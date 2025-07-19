using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Team;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class OnlinePairCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public OnlinePairCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        _context.OnlinePairDbSet
            .Where(x => x.CardProfile == cardProfile)
            .ToList()
            .ForEach(onlinePair =>
            {
                var team = _context.TagTeamDataDbSet
                    .FirstOrDefault(tagTeam => tagTeam.Id == onlinePair.TeamId);

                if (team is null)
                {
                    return;
                }

                var partnerId = GetPartnerId(cardProfile, team);

                var partnerProfile = _context.CardProfiles
                    .Include(x => x.PlayerLevel)
                    .Include(x => x.TeamClassMatchRecord)
                    .FirstOrDefault(x => x.Id == partnerId);

                if (partnerProfile is null)
                {
                    return;
                }

                mobileUserGroup.online_tag_info.TagTeamPartners.Add(
                    new Response.LoadCard.MobileUserGroup.OnlineTagInfo.OnlineTagPartner
                    {
                        Id = (uint)team.Id,
                        Name = partnerProfile.UserName,
                        PlayerLevelId = partnerProfile.PlayerLevel.PlayerLevelId,
                        PrestigeId = partnerProfile.PlayerLevel.PrestigeId,
                        ClassIdTeam = partnerProfile.TeamClassMatchRecord.ClassId
                    });
            });
    }

    private int GetPartnerId(CardProfile cardProfile, TagTeamData team)
    {
        var partnerId = 0;

        if (team.CardId != cardProfile.Id)
        {
            partnerId = team.CardId;
        }

        if (team.TeammateCardId != cardProfile.Id)
        {
            partnerId = (int)team.TeammateCardId;
        }

        return partnerId;
    }
}