using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Models.Cards.Titles.User;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Card;

public record UpdateBasicProfileCommand(UpdateBasicProfileRequest Request) : IRequest<BasicResponse>;

public class UpdateBasicProfileCommandHandler : IRequestHandler<UpdateBasicProfileCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpdateBasicProfileCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateBasicProfileCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.BasicProfile == null)
        {
            throw new InvalidRequestDataException("Basic Profile is null");
        }

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        var basicProfile = updateRequest.BasicProfile;
        
        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var userTitleSetting = _context.UserTitleSettingDbSet
            .Include(x => x.UserDefaultTitle)
            .Include(x => x.UserTriadTitle)
            .Include(x => x.UserClassMatchTitle)
            .First(x => x.CardProfile == cardProfile);
        
        var customizeProfile = _context.CustomizeProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var generalSetting = _context.GeneralSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        cardProfile.UserName = basicProfile.UserName;
        playerProfile.OpenEchelon = basicProfile.OpenEchelon;
        playerProfile.OpenRecord = basicProfile.OpenRecord;

        UpdateTitle(userTitleSetting.UserDefaultTitle, basicProfile.DefaultTitle);
        UpdateTitle(userTitleSetting.UserTriadTitle, basicProfile.TriadTitle);
        UpdateTitle(userTitleSetting.UserClassMatchTitle, basicProfile.ClassMatchTitle);

        customizeProfile.DefaultGaugeDesignId = basicProfile.DefaultGaugeDesignId;
        customizeProfile.DefaultBgmPlayMethod = (uint) basicProfile.DefaultBgmPlayingMethod;
        customizeProfile.DefaultBgmSettings = String.Join(",", basicProfile.DefaultBgmList.ToArray());
        customizeProfile.StageRandoms = String.Join(",", basicProfile.StageRandoms.ToArray());

        generalSetting.FixPositionRadar = basicProfile.IsFixedRadar;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
    
    void UpdateTitle(UserTitle userTitle, Title title)
    {
        userTitle.TitleTextId = title.TextId;
        userTitle.TitleOrnamentId = title.OrnamentId;
        userTitle.TitleEffectId = title.EffectId;
        userTitle.TitleBackgroundPartsId = title.BackgroundPartsId;
        userTitle.CustomText = title.CustomText;
    }
}