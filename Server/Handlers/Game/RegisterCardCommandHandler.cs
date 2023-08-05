using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;

namespace Server.Handlers.Game;

public record RegisterCardCommand(Request Request) : IRequest<Response>;

public class RegisterCardCommandHandler : IRequestHandler<RegisterCardCommand, Response>
{
    private readonly ILogger<RegisterCardCommandHandler> logger;
    private readonly ServerDbContext _context;
    
    public RegisterCardCommandHandler(ILogger<RegisterCardCommandHandler> logger, ServerDbContext context)
    {
        this.logger = logger;
        this._context = context;
    }
    
    public Task<Response> Handle(RegisterCardCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var registerCardRequest = request.register_card;
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == registerCardRequest.AccessCode && x.SessionId == registerCardRequest.SessionId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }

        var pilotId = cardProfile.PilotDomain.PilotId;
        var userId = cardProfile.UserDomain.UserId;

        cardProfile.PilotDomain.LoadPlayerJson = JsonConvert.SerializeObject(CreateLoadPlayer(Convert.ToUInt32(pilotId)));
        cardProfile.PilotDomain.PilotDataGroupJson = JsonConvert.SerializeObject(CreatePilotDataGroup());
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(CreatePerLoadMobileUserGroup(Convert.ToUInt32(userId)));
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(CreateMobileUserGroup(Convert.ToUInt32(userId)));
        cardProfile.IsNewCard = false;
        
        _context.SaveChanges();
        
        response.register_card = new Response.RegisterCard
        {
            SessionId = registerCardRequest.SessionId,
            AccessCode = registerCardRequest.AccessCode,
            AcidError = AcidError.AcidNoUse,
            IsRegistered = true,
            PilotId = Convert.ToUInt32(pilotId)
        };
        
        return Task.FromResult(response);
    }

    Response.PreLoadCard.LoadPlayer CreateLoadPlayer(uint pilotId)
    {
        return new Response.PreLoadCard.LoadPlayer
        {
            PilotId = Convert.ToUInt32(pilotId),
            TotalWin = 0,
            TotalLose = 0,
            EchelonId = 0,
            EchelonExp = 0,
            SEchelonFlag = false,
            SEchelonMissionFlag = false,
            SEchelonProgress = 0,
            SCaptainFlag = false,
            SBrigadierFlag = false,
            VsmAfterRankUp = 0,
            ShuffleWin = 0,
            ShuffleLose = 0,
            TeamWin = 0,
            TeamLose = 0,
            RankSoloWin = 0,
            RankSoloLose = 0,
            RankTeamWin = 0,
            RankTeamLose = 0,
            CasualSoloWin = 0,
            CasualSoloLose = 0,
            CasualTeamWin = 0,
            CasualTeamLose = 0,
            RankIdSolo = 0,
            RankIdTeam = 0,
            AchievedExNumSolo = 0,
            AchievedExxNumSolo = 0,
            AchievedExNumTeam = 0,
            AchievedExxNumTeam = 0
        };
    }

    Response.LoadCard.PilotDataGroup CreatePilotDataGroup()
    {
        return new Response.LoadCard.PilotDataGroup {
            RewardMsIds = null,
            MsSkills = {},
            TagTeams = {},
            CpuScenes = {},
            CpuRibbons = {},
            TotalTriadScore = 0,
            TotalTriadWantedDefeatNum = 0,
            Training = new Response.LoadCard.PilotDataGroup.TrainingSettingGroup{
                MstMobileSuitId = 1,
                BurstType = 1,
                CpuLevel = 1,
                ExBurstGauge= 0,
                DamageDisplay = true,
                CpuAutoGuard = false,
                CommandGuideDisplay = true
            },
            TotalTriadScenePlayNum = 0,
            pilot_rank_match = new Response.LoadCard.PilotDataGroup.PilotRankMatch
            {
                PilotRankMatchSolo = CreateNewPilotRankMatchInfo(),
                PilotRankMatchTeam = CreateNewPilotRankMatchInfo()
            },
        };
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

    Response.PreLoadCard.MobileUserGroup CreatePerLoadMobileUserGroup(uint userId)
    {
        return new Response.PreLoadCard.MobileUserGroup
        {
            UserId = userId,
            PlayerName = "EXVS2-" + GetRandomAlphaNumeric(6),
            OpenRecord = 1,
            OpenEchelon = 0,
            OpenSkillpoint = true,
            customize_group = new Response.PreLoadCard.MobileUserGroup.CustomizeGroup {
                DefaultTitleCustomize = new TitleCustomize {
                    TitleTextId = 0,
                    TitleOrnamentId = 0,
                    TitleEffectId = 0,
                    TitleBackgroundPartsId = 0
                },
                MstMobileSuitId = 0,
                MsSkill1 = 0,
                MsSkill2 = 0,
                GpBoost = 1,
                GuestNavBoost = 1,
                TagSkillPointBoostFlag = true,
                BattleNavAdviseFlag = true,
                BattleNavNotifyFlag = true,
                RandomTitleFlag = false,
                GuestNavAnnivFlag = false
            },
            GuestNavs = {},
            FavoriteMobileSuits = {},
            ChallengeMisDatas = {},
            KeyconfigNumber = 1,
            Gp = 0
        };
    }

    Response.LoadCard.MobileUserGroup CreateMobileUserGroup(uint userId)
    {
        return new Response.LoadCard.MobileUserGroup
        {
            Customize = new Response.LoadCard.MobileUserGroup.CustomizeGroup {
                DefaultGaugeDesignId = 0,
                DefaultBgmSettings = Array.Empty<uint>(),
                DefaultBgmPlayMethod = 0,
                StageRandoms = null,
                BasePanelId = 0,
                CommentPartsAId = 0,
                CommentPartsBId = 0
            },
            PlayingMessages = {},
            ResultMessages = {},
            OnlineShufflePlayingMessages = {},
            OnlineShuffleResultMessages = {},
            MstMobileSuitId = 0,
            ArmorLevel = 0,
            ShootAttackLevel = 0,
            InfightAttackLevel = 0,
            BoosterLevel = 0,
            ExGaugeLevel = 0,
            AiLevel = 0,
            OpeningMessages = {},
            OnlineShuffleOpeningMessages = {},
            BurstType = 3,
            PaidFlag = true,
            PcoinTicketNum = 99,
            MessagePosition = 0,
            PlayingStampFlag = false,
            TriadTeamName = "EXVS2XB",
            TriadBackgroundPartsId = 0,
            RankMatchTitleCustomize = new TitleCustomize {
                TitleTextId = 0,
                TitleOrnamentId = 0,
                TitleEffectId = 0,
                TitleBackgroundPartsId = 0
            },
            TriadTitleCustomize = new TitleCustomize {
                TitleTextId = 0,
                TitleOrnamentId = 0,
                TitleEffectId = 0,
                TitleBackgroundPartsId = 0
            },
            MobileUserId = userId,
            PartnerRecruitAmIdNglists = null
        };
    }
    
    string GetRandomAlphaNumeric(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var list = Enumerable.Repeat(0, length).Select(x=>chars[random.Next(chars.Length)]);
        return string.Join("", list);
    }
}