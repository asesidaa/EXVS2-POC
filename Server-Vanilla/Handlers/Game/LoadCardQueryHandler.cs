using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerVanilla.Mapper.Card.MobileSuit;
using ServerVanilla.Mapper.Card.Setting;
using ServerVanilla.Mapper.Card.Triad;
using ServerVanilla.Models.Cards;
using ServerVanilla.Models.Cards.Message;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record LoadCardQuery(Request Request) : IRequest<Response>;

public class LoadCardQueryHandler : IRequestHandler<LoadCardQuery, Response>
{
    private readonly ILogger<LoadCardQueryHandler> _logger;
    private readonly ServerDbContext _context;
    private const uint TagSkillPointBoost = 50; // Will be divided by 5 in game

    public LoadCardQueryHandler(ILogger<LoadCardQueryHandler> logger, ServerDbContext context)
    {
        _logger = logger;
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
            .Include(x => x.TriadMiscInfo)
            .Include(x => x.TriadCourseDatas)
            .Include(x => x.TrainingProfile)
            .Include(x => x.MobileSuits)
            .Include(x => x.TriadPartner)
            .Include(x => x.CustomizeProfile)
            .Include(x => x.TagTeamDatas)
            .Include(x => x.GamepadSetting)
            .Include(x => x.OpeningMessage)
            .Include(x => x.PlayingMessage)
            .Include(x => x.ResultMessage)
            .Include(x => x.OnlineOpeningMessage)
            .Include(x => x.OnlinePlayingMessage)
            .Include(x => x.OnlineResultMessage)
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            response.Error = Error.ErrServer;
            return Task.FromResult(response);
        }

        var cpuRibbons = new uint[] { };

        if (cardProfile.TriadMiscInfo.CpuRibbons != string.Empty)
        {
            cpuRibbons = Array.ConvertAll(cardProfile.TriadMiscInfo.CpuRibbons.Split(','), Convert.ToUInt32);
        }

        var pilotDataGroup = new Response.LoadCard.PilotDataGroup()
        {
            RewardMsIds = null,
            CpuRibbons = cpuRibbons,
            TotalTriadScore = cardProfile.TriadMiscInfo.TotalTriadScore,
            TotalTriadWantedDefeatNum = cardProfile.TriadMiscInfo.TotalTriadWantedDefeatNum,
            TotalTriadScenePlayNum = cardProfile.TriadMiscInfo.TotalTriadScenePlayNum,
            Training = new Response.LoadCard.PilotDataGroup.TrainingSettingGroup()
            {
                MstMobileSuitId = cardProfile.TrainingProfile.MstMobileSuitId,
                BurstType = cardProfile.TrainingProfile.BurstType,
                CpuLevel = cardProfile.TrainingProfile.CpuLevel,
                ExBurstGauge = cardProfile.TrainingProfile.ExBurstGauge,
                DamageDisplay = cardProfile.TrainingProfile.DamageDisplay,
            }
        };
        
        cardProfile.TriadCourseDatas.ToList()
            .ForEach(triadCourseData => pilotDataGroup.CpuScenes.Add(triadCourseData.ToCpuSceneData()));
        
        cardProfile.MobileSuits.ToList()
            .ForEach(mobileSuitUsage => pilotDataGroup.MsSkills.Add(mobileSuitUsage.ToMSSkillGroup()));
        
        AppendTagTeams(cardProfile, pilotDataGroup);

        var defaultBgmSettings = new uint[] { };
        
        if (cardProfile.CustomizeProfile.DefaultBgmSettings != string.Empty)
        {
            defaultBgmSettings = Array.ConvertAll(cardProfile.CustomizeProfile.DefaultBgmSettings.Split(','), uint.Parse);
        }
        
        var mobileUserGroup = new Response.LoadCard.MobileUserGroup()
        {
            Customize = new Response.LoadCard.MobileUserGroup.CustomizeGroup()
            {
                DefaultGaugeDesignId = cardProfile.CustomizeProfile.DefaultGaugeDesignId,
                DefaultBgmPlayMethod = cardProfile.CustomizeProfile.DefaultBgmPlayMethod,
                DefaultBgmSettings = defaultBgmSettings,
                BasePanelId = cardProfile.CustomizeProfile.BasePanelId,
                CommentPartsAId = cardProfile.CustomizeProfile.CommentPartsAId,
                CommentPartsBId = cardProfile.CustomizeProfile.CommentPartsBId
            },
            // CPU Partner Domain
            MstMobileSuitId = cardProfile.TriadPartner.MstMobileSuitId,
            ArmorLevel = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.ArmorLevel : 0,
            ShootAttackLevel = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.ShootAttackLevel : 0,
            InfightAttackLevel = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.InfightAttackLevel : 0,
            BoosterLevel = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.BoosterLevel : 0,
            ExGaugeLevel = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.ExGaugeLevel : 0,
            AiLevel = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.AiLevel : 0,
            BurstType = cardProfile.TriadPartner.MstMobileSuitId > 0 ? cardProfile.TriadPartner.BurstType : 0,
            Gamepad = cardProfile.GamepadSetting.ToGamepadGroup()
        };
        
        mobileUserGroup.OpeningMessages.AddRange(CreateCommandMessageGroups(
            cardProfile.OpeningMessage,
            WebUIVanilla.Shared.Dto.Enum.Command.StartUp, WebUIVanilla.Shared.Dto.Enum.Command.StartDown,
            WebUIVanilla.Shared.Dto.Enum.Command.StartLeft, WebUIVanilla.Shared.Dto.Enum.Command.StartRight
        ));
        
        AppendMessages(mobileUserGroup, cardProfile);

        var loadCard = new Response.LoadCard()
        {
            pilot_data_group = pilotDataGroup,
            mobile_user_group = mobileUserGroup
        };

        response.load_card = loadCard;
        
        return Task.FromResult(response);
    }

    private void AppendTagTeams(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        if (cardProfile.TagTeamDatas.Count > 0)
        {
            cardProfile.TagTeamDatas
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
                        TagRate = tagTeamData.TagRate,
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
                TagRate = tagTeamData.TagRate,
                BackgroundPartsId = tagTeamData.BackgroundPartsId,
                EffectId = tagTeamData.EffectId,
                EmblemId = tagTeamData.EmblemId,
                BgmId = tagTeamData.BgmId,
                NameColorId = tagTeamData.NameColorId
            });
        });
    }
    
    void AppendMessages(Response.LoadCard.MobileUserGroup mobileUserGroup, CardProfile cardProfile)
    {
        mobileUserGroup.PlayingMessages.AddRange(CreateCommandMessageGroups(
            cardProfile.PlayingMessage,
            WebUIVanilla.Shared.Dto.Enum.Command.Up, WebUIVanilla.Shared.Dto.Enum.Command.Down,
            WebUIVanilla.Shared.Dto.Enum.Command.Left, WebUIVanilla.Shared.Dto.Enum.Command.Right
        ));

        mobileUserGroup.ResultMessages.AddRange(CreateCommandMessageGroups(
            cardProfile.ResultMessage,
            WebUIVanilla.Shared.Dto.Enum.Command.ResultUp, WebUIVanilla.Shared.Dto.Enum.Command.ResultDown,
            WebUIVanilla.Shared.Dto.Enum.Command.ResultLeft, WebUIVanilla.Shared.Dto.Enum.Command.ResultRight
        ));

        mobileUserGroup.OnlineShuffleOpeningMessages.AddRange(CreateCommandMessageGroups(
            cardProfile.OnlineOpeningMessage,
            WebUIVanilla.Shared.Dto.Enum.Command.StartUp, WebUIVanilla.Shared.Dto.Enum.Command.StartDown,
            WebUIVanilla.Shared.Dto.Enum.Command.StartLeft, WebUIVanilla.Shared.Dto.Enum.Command.StartRight
        ));

        mobileUserGroup.OnlineShufflePlayingMessages.AddRange(CreateCommandMessageGroups(
            cardProfile.OnlinePlayingMessage,
            WebUIVanilla.Shared.Dto.Enum.Command.Up, WebUIVanilla.Shared.Dto.Enum.Command.Down,
            WebUIVanilla.Shared.Dto.Enum.Command.Left, WebUIVanilla.Shared.Dto.Enum.Command.Right
        ));

        mobileUserGroup.OnlineShuffleResultMessages.AddRange(CreateCommandMessageGroups(
            cardProfile.OnlineResultMessage,
            WebUIVanilla.Shared.Dto.Enum.Command.ResultUp, WebUIVanilla.Shared.Dto.Enum.Command.ResultDown,
            WebUIVanilla.Shared.Dto.Enum.Command.ResultLeft, WebUIVanilla.Shared.Dto.Enum.Command.ResultRight
        ));
    }
    
    List<Response.LoadCard.MobileUserGroup.CommandMessageGroup> CreateCommandMessageGroups(Message message, 
        WebUIVanilla.Shared.Dto.Enum.Command upDirection,
        WebUIVanilla.Shared.Dto.Enum.Command downDirection,
        WebUIVanilla.Shared.Dto.Enum.Command leftDirection,
        WebUIVanilla.Shared.Dto.Enum.Command rightDirection)
    {
        var commandMessageGroups = new List<Response.LoadCard.MobileUserGroup.CommandMessageGroup>();
        
        if (message.TopMessageText != string.Empty || message.TopUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.TopMessageText, message.TopUniqueMessageId, upDirection));
        }
        
        if (message.DownMessageText != string.Empty || message.DownUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.DownMessageText, message.DownUniqueMessageId, downDirection));
        }
        
        if (message.LeftMessageText != string.Empty || message.LeftUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.LeftMessageText, message.LeftUniqueMessageId, leftDirection));
        }
        
        if (message.RightMessageText != string.Empty || message.RightUniqueMessageId > 0)
        {
            commandMessageGroups.Add(CreateCommandMessageGroup(message.RightMessageText, message.RightUniqueMessageId, rightDirection));
        }

        return commandMessageGroups;
    }

    Response.LoadCard.MobileUserGroup.CommandMessageGroup CreateCommandMessageGroup(string message, uint stampId,
        WebUIVanilla.Shared.Dto.Enum.Command direction)
    {
        return new Response.LoadCard.MobileUserGroup.CommandMessageGroup()
        {
            Command = (uint)direction,
            MessageText = message,
            UniqueMessageId = stampId
        };
    }
}