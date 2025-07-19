using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServerOver.Mapper.Card.Titles;
using ServerOver.Models.Cards;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using ServerOver.Utils;
using WebUIOver.Shared.Dto.Enum;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Card;

public record GetBasicDisplayProfileCommand(string AccessCode, string ChipId) : IRequest<BasicDisplayProfile>;

public class GetBasicDisplayProfileCommandHandler : IRequestHandler<GetBasicDisplayProfileCommand, BasicDisplayProfile>
{
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;

    public GetBasicDisplayProfileCommandHandler(ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _context = context;
        _config = options.Value;
    }
    
    public Task<BasicDisplayProfile> Handle(GetBasicDisplayProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var customizeProfile = _context.CustomizeProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var userTitleSetting = _context.UserTitleSettingDbSet
            .Include(x => x.UserDefaultTitle)
            .Include(x => x.UserTriadTitle)
            .Include(x => x.UserClassMatchTitle)
            .First(x => x.CardProfile == cardProfile);

        var generalSetting = _context.GeneralSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        var stageRandoms = Array.ConvertAll(ArrayUtil.FromString(customizeProfile.StageRandoms), Convert.ToUInt32).ToList();
        
        for (var i = stageRandoms.Count - 1; i < 10; i++)
        {
            stageRandoms.Add(0u);
        }
        
        var basicDisplayProfile = new BasicDisplayProfile
        {
            UserId = (uint) cardProfile.Id,
            UserName = cardProfile.UserName,
            OpenEchelon = playerProfile.OpenEchelon,
            OpenRecord = playerProfile.OpenRecord,
            DefaultGaugeDesignId = customizeProfile.DefaultGaugeDesignId,
            DefaultBgmPlayingMethod = (BgmPlayingMethod) customizeProfile.DefaultBgmPlayMethod,
            DefaultBgmList = Array.ConvertAll(ArrayUtil.FromString(customizeProfile.DefaultBgmSettings), Convert.ToUInt32),
            StageRandoms = stageRandoms.ToArray(),
            DefaultTitle = userTitleSetting.UserDefaultTitle.ToTitle(),
            TriadTitle = userTitleSetting.UserTriadTitle.ToTitle(),
            ClassMatchTitle = userTitleSetting.UserClassMatchTitle.ToTitle(),
            IsFixedRadar = generalSetting.FixPositionRadar,
            Gp = cardProfile.Gp
        };

        return Task.FromResult(basicDisplayProfile);
    }
}