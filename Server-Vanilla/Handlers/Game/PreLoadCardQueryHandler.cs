using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerVanilla.Mapper.Card.Navi;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record PreLoadCardQuery(Request Request) : IRequest<Response>;

public class PreLoadCardQueryHandler : IRequestHandler<PreLoadCardQuery, Response>
{
    private readonly ILogger<PreLoadCardQueryHandler> _logger;
    private readonly ServerDbContext _context;

    public PreLoadCardQueryHandler(ILogger<PreLoadCardQueryHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Response> Handle(PreLoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };

        var preLoadCardRequest = request.pre_load_card;
        
        var cardProfile = await _context.CardProfiles
            .Include(x => x.PlayerProfile)
            .Include(x => x.BattleProfile)
            .Include(x => x.Navi)
            .Include(x => x.FavouriteMobileSuits)
            .Include(x => x.MobileSuits)
            .Include(x => x.TriadPartner)
            .Include(x => x.DefaultTitle)
            .Include(x => x.BoostSetting)
            .Include(x => x.NaviSetting)
            .FirstOrDefaultAsync(x => x.AccessCode == preLoadCardRequest.AccessCode && x.ChipId == preLoadCardRequest.ChipId, cancellationToken);
        
        var sessionId = preLoadCardRequest.AccessCode + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        if (cardProfile != null)
        {
            return await ReadAndReturn(preLoadCardRequest, cardProfile, sessionId, response);
        }
        
        _logger.LogInformation("Card not exist for ChipId = {ChipId}, Now creating...", preLoadCardRequest.ChipId);

        var newCardProfile = new CardProfile()
        {
            AccessCode = preLoadCardRequest.AccessCode,
            ChipId = preLoadCardRequest.ChipId,
            SessionId = sessionId,
            UserName = "EXVS2-" + GetRandomAlphaNumeric(6),
            IsNewCard = true,
            DistinctTeamFormationToken = Guid.NewGuid().ToString("n").Substring(0, 16),
            LastPlayedAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds()
        };
        
        _context.CardProfiles.Add(newCardProfile);
        await _context.SaveChangesAsync(cancellationToken);
        
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            IsNewCard = true
        };
        
        return response;
    }

    private async Task<Response> ReadAndReturn(Request.PreLoadCard preLoadCardRequest, CardProfile cardProfile,
        string sessionId, Response response)
    {
        _logger.LogInformation("Card exists for ChipId = {ChipId}, Now reading from Database", preLoadCardRequest.ChipId);
        cardProfile.SessionId = sessionId;
        await _context.SaveChangesAsync();
        
        if (cardProfile.IsNewCard)
        {
            _logger.LogInformation("ChipId = {} is still a new card, will go to RegisterCard", preLoadCardRequest.ChipId);
            response.pre_load_card = new Response.PreLoadCard
            {
                SessionId = sessionId,
                AcidResponse = null,
                AcidError = AcidError.AcidSuccess,
                IsNewCard = true
            };
            return response;
        }
        
        var preLoadCard = new Response.PreLoadCard.MobileUserGroup()
        {
            UserId = (uint) cardProfile.Id,
            PlayerName = cardProfile.UserName,
            OpenRecord = cardProfile.PlayerProfile.OpenRecord,
            OpenEchelon = cardProfile.PlayerProfile.OpenEchelon,
            OpenSkillpoint = cardProfile.PlayerProfile.OpenSkillpoint,
            KeyconfigNumber = 1,
            HeadphoneFlag = false,
            Gp = cardProfile.Gp
        };
        
        cardProfile.Navi.ToList().ForEach(navi => preLoadCard.GuestNavs.Add(navi.ToGuestNaviGroup()));
        cardProfile.FavouriteMobileSuits
            .OrderBy(favouriteMs => favouriteMs.Id)
            .ToList()
            .ForEach(favouriteMs =>
            {
                var bgmSettings = new uint[] { };

                if (favouriteMs.BgmSettings != string.Empty)
                {
                    bgmSettings = Array.ConvertAll(favouriteMs.BgmSettings.Split(','), uint.Parse);
                }
                
                var favouriteMsGroup = new Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup();
                favouriteMsGroup.MstMobileSuitId = favouriteMs.MstMobileSuitId;
                favouriteMsGroup.MsUsedNum = 0;
                favouriteMsGroup.OpenSkillpoint = favouriteMs.OpenSkillpoint;
                favouriteMsGroup.GaugeDesignId = favouriteMs.GaugeDesignId;
                favouriteMsGroup.BgmSettings = bgmSettings;
                favouriteMsGroup.BgmPlayMethod = favouriteMs.BgmPlayMethod;
                favouriteMsGroup.BattleNavId = favouriteMs.BattleNavId;
                favouriteMsGroup.BurstType = favouriteMs.BurstType;

                var msUsage = cardProfile.MobileSuits
                    .FirstOrDefault(ms => ms.MstMobileSuitId == favouriteMs.MstMobileSuitId);

                if (msUsage is not null)
                {
                    favouriteMsGroup.MsUsedNum = msUsage.MsUsedNum;
                }
                
                preLoadCard.FavoriteMobileSuits.Add(favouriteMsGroup);
            });

        var customizeGroup = new Response.PreLoadCard.MobileUserGroup.CustomizeGroup();
        
        customizeGroup.TitleTextId = cardProfile.DefaultTitle.TitleTextId;
        customizeGroup.TitleOrnamentId = cardProfile.DefaultTitle.TitleOrnamentId;
        customizeGroup.TitleEffectId = cardProfile.DefaultTitle.TitleEffectId;
        customizeGroup.TitleBackgroundPartsId = cardProfile.DefaultTitle.TitleBackgroundPartsId;
        
        customizeGroup.MstMobileSuitId = cardProfile.TriadPartner.MstMobileSuitId;
        customizeGroup.MsSkill1 = customizeGroup.MstMobileSuitId > 0 ? cardProfile.TriadPartner.MsSkill1 : 0;
        customizeGroup.MsSkill2 = customizeGroup.MstMobileSuitId > 0 ? cardProfile.TriadPartner.MsSkill2 : 0;
        
        customizeGroup.GpBoost = cardProfile.BoostSetting.GpBoost;
        customizeGroup.GuestNavBoost = cardProfile.BoostSetting.GuestNavBoost;
        
        customizeGroup.TagSkillPointBoostFlag = true;
        
        customizeGroup.BattleNavAdviseFlag = cardProfile.NaviSetting.BattleNavAdviseFlag;
        customizeGroup.BattleNavNotifyFlag = cardProfile.NaviSetting.BattleNavNotifyFlag;

        preLoadCard.customize_group = customizeGroup;
        
        var loadPlayer = new Response.PreLoadCard.LoadPlayer()
        {
            PilotId = (uint) cardProfile.Id,
            LastPlayedAt = cardProfile.LastPlayedAt,
            TotalWin = cardProfile.BattleProfile.TotalWin,
            TotalLose = cardProfile.BattleProfile.TotalLose,
            EchelonId = cardProfile.BattleProfile.EchelonId,
            EchelonExp = cardProfile.BattleProfile.EchelonExp,
            SEchelonFlag = cardProfile.BattleProfile.SEchelonFlag,
            SEchelonMissionFlag = cardProfile.BattleProfile.SEchelonMissionFlag,
            SEchelonProgress = cardProfile.BattleProfile.SEchelonProgress,
            SCaptainFlag = cardProfile.BattleProfile.SCaptainFlag,
            SBrigadierFlag = cardProfile.BattleProfile.SBrigadierFlag,
            MatchingCorrectionSolo = cardProfile.BattleProfile.MatchingCorrectionSolo,
            MatchingCorrectionTeam = cardProfile.BattleProfile.MatchingCorrectionTeam,
            VsmAfterRankUp = cardProfile.BattleProfile.VsmAfterRankUp,
            ShuffleWin = cardProfile.BattleProfile.ShuffleWin,
            ShuffleLose = cardProfile.BattleProfile.ShuffleLose,
            TeamWin = cardProfile.BattleProfile.TeamWin,
            TeamLose = cardProfile.BattleProfile.TeamLose,
        };
        
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            AmId = 1,
            IsNewCard = false,
            User = preLoadCard,
            load_player = loadPlayer
        };
        
        return response;
    }

    string GetRandomAlphaNumeric(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var list = Enumerable.Repeat(0, length).Select(x=>chars[random.Next(chars.Length)]);
        return string.Join("", list);
    }
}