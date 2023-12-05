using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using Serilog;
using Server.Persistence;
using System.Text.Json;
using Newtonsoft.Json;
using Server.Models.Cards;

namespace Server.Handlers.Game;

public record LoadCardQuery(Request Request) : IRequest<Response>;

public class LoadCardQueryHandler : IRequestHandler<LoadCardQuery, Response>
{
    private readonly ServerDbContext _context;
    private const uint TagSkillPointBoost = 5; // Will be divided by 5 in game

    public LoadCardQueryHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<Response> Handle(LoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
        };

        var sessionId = request.load_card.SessionId;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.UserDomain)
            .Include(x => x.OnlinePairs)
            .Include(x => x.TagTeamDataList)
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            response.Error = Error.ErrServer;
            return Task.FromResult(response);
        }

        var pilotDataGroup =
            JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);
        var mobileUserGroup =
            JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain
                .MobileUserGroupJson);
        
        if (mobileUserGroup is not null)
        {
            mobileUserGroup.online_tag_info = new Response.LoadCard.MobileUserGroup.OnlineTagInfo
            {
                quick_tag_partner = null
            };
        
            var legitOnlinePairs = cardProfile.OnlinePairs
                .Where(x => x.CardId != 0 && x.PairId != 0)
                .ToList();

            if (legitOnlinePairs.Count > 0)
            {
                AddOnlinePairs(cardProfile, legitOnlinePairs, mobileUserGroup);
            }

            // For Online Quick Tag
            SetQuickTagPartner(cardProfile, mobileUserGroup);
        }

        if (pilotDataGroup is not null)
        {
            if (pilotDataGroup.pilot_rank_match is null)
            {
                PatchRankingDataIfNotExist(pilotDataGroup, cardProfile);
            }

            // For Offline / Online Teams
            pilotDataGroup.TagTeams.Clear();
            AppendTagTeams(cardProfile, pilotDataGroup);
        }

        response.load_card = new Response.LoadCard
        {
            pilot_data_group = pilotDataGroup,
            mobile_user_group = mobileUserGroup,
        };

        //String readStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"));
        //response.load_card = JsonConvert.DeserializeObject<Response.LoadCard>(loadCard);

        return Task.FromResult(response);
    }

    private void AppendTagTeams(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        if (cardProfile.TagTeamDataList.Count > 0)
        {
            cardProfile.TagTeamDataList
                .ToList()
                .ForEach(tagTeamData =>
                {
                    pilotDataGroup.TagTeams.Add(new TagTeamGroup()
                    {
                        Id = (uint)tagTeamData.Id,
                        Name = tagTeamData.TeamName,
                        PartnerId = tagTeamData.TeammateCardId,
                        SkillPoint = tagTeamData.SkillPoint,
                        SkillPointBoost = TagSkillPointBoost,
                        BackgroundPartsId = tagTeamData.BackgroundPartsId,
                        EffectId = tagTeamData.EffectId,
                        EmblemId = tagTeamData.EmblemId,
                        BgmId = tagTeamData.BgmId,
                        NameColorId = tagTeamData.NameColorId
                    });
                });
        }

        var oppositeTagTeamDatas = _context.TagTeamData
            .Where(tagTeamData => tagTeamData.TeammateCardId == cardProfile.Id)
            .ToList();
        
        oppositeTagTeamDatas.ForEach(tagTeamData =>
        {
            pilotDataGroup.TagTeams.Add(new TagTeamGroup()
            {
                Id = (uint)tagTeamData.Id,
                Name = tagTeamData.TeamName,
                PartnerId = (uint)tagTeamData.CardId,
                SkillPoint = tagTeamData.SkillPoint,
                SkillPointBoost = TagSkillPointBoost,
                BackgroundPartsId = tagTeamData.BackgroundPartsId,
                EffectId = tagTeamData.EffectId,
                EmblemId = tagTeamData.EmblemId,
                BgmId = tagTeamData.BgmId,
                NameColorId = tagTeamData.NameColorId
            });
        });
    }

    private void PatchRankingDataIfNotExist(Response.LoadCard.PilotDataGroup pilotDataGroup, CardProfile cardProfile)
    {
        pilotDataGroup.pilot_rank_match = new Response.LoadCard.PilotDataGroup.PilotRankMatch
        {
            PilotRankMatchSolo = CreateNewPilotRankMatchInfo(),
            PilotRankMatchTeam = CreateNewPilotRankMatchInfo()
        };
        cardProfile.PilotDomain.PilotDataGroupJson = JsonConvert.SerializeObject(pilotDataGroup);
        _context.SaveChanges();
    }

    private void SetQuickTagPartner(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var quickTagProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.Id == cardProfile.QuickOnlinePartnerId);

        if (quickTagProfile is null)
        {
            return;
        }

        var quickTagPilotDataGroup =
            JsonConvert.DeserializeObject<Response.PreLoadCard.LoadPlayer>(quickTagProfile.PilotDomain.LoadPlayerJson);
        var quickTagMobileUserGroup =
            JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(quickTagProfile.UserDomain.UserJson);

        if (quickTagPilotDataGroup is null)
        {
            return;
        }

        if (quickTagMobileUserGroup is null)
        {
            return;
        }

        mobileUserGroup.online_tag_info.quick_tag_partner =
            new Response.LoadCard.MobileUserGroup.OnlineTagInfo.QuickTagPartner
            {
                Id = (uint)quickTagProfile.Id,
                Name = quickTagMobileUserGroup.PlayerName,
                EchelonId = quickTagPilotDataGroup.EchelonId,
                SEchelonFlag = quickTagPilotDataGroup.SEchelonFlag,
                RankIdTeam = quickTagPilotDataGroup.RankIdTeam,
                AchievedExNumTeam = quickTagPilotDataGroup.AchievedExNumTeam,
                AchievedExxNumTeam = quickTagPilotDataGroup.AchievedExxNumTeam
            };
    }

    private void AddOnlinePairs(CardProfile cardProfile, List<OnlinePair> legitOnlinePairs, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        legitOnlinePairs.ForEach(onlinePair =>
        {
            var team = _context.TagTeamData.FirstOrDefault(tagTeam => tagTeam.Id == onlinePair.TeamId);

            if (team is null)
            {
                return;
            }

            var partnerId = 0;

            if (team.CardId != cardProfile.Id)
            {
                partnerId = team.CardId;
            }
            
            if (team.TeammateCardId != cardProfile.Id)
            {
                partnerId = (int) team.TeammateCardId;
            }
            
            var partnerProfile = _context.CardProfiles
                .Include(x => x.PilotDomain)
                .Include(x => x.UserDomain)
                .FirstOrDefault(x => x.Id == partnerId);

            if (partnerProfile is null)
            {
                return;
            }

            var partnerPilotDataGroup =
                JsonConvert.DeserializeObject<Response.PreLoadCard.LoadPlayer>(partnerProfile.PilotDomain.LoadPlayerJson);
            var partnerMobileUserGroup =
                JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(partnerProfile.UserDomain.UserJson);

            if (partnerPilotDataGroup is null)
            {
                return;
            }

            if (partnerMobileUserGroup is null)
            {
                return;
            }

            mobileUserGroup.online_tag_info.TagTeamPartners.Add(
                new Response.LoadCard.MobileUserGroup.OnlineTagInfo.OnlineTagPartner
                {
                    Id = (uint) team.Id,
                    Name = partnerMobileUserGroup.PlayerName,
                    EchelonId = partnerPilotDataGroup.EchelonId,
                    SEchelonFlag = partnerPilotDataGroup.SEchelonFlag,
                    RankIdTeam = partnerPilotDataGroup.RankIdTeam,
                    AchievedExNumTeam = partnerPilotDataGroup.AchievedExNumTeam,
                    AchievedExxNumTeam = partnerPilotDataGroup.AchievedExxNumTeam
                });
        });
    }
    
    Response.LoadCard.PilotDataGroup.PilotRankMatch.PilotRankMatchInfo CreateNewPilotRankMatchInfo()
    {
        return new Response.LoadCard.PilotDataGroup.PilotRankMatch.PilotRankMatchInfo
        {
            RankId = 0,
            Level = 0,
            WinLoseInfoes = new uint[] {},
            RankPoint = 0,
            ExRank = 0,
            ExRankChangeFlag = 0,
            CpuNum = 0,
            ExxLockFlag = false,
            PreTrialExxFlag = false
        };
    }
}