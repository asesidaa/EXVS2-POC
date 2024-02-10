using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Profile;

public record UpdateBasicProfileCommand(UpdateBasicProfileRequest Request) : IRequest<BasicResponse>;

public class UpdateBasicProfileCommandHandler : IRequestHandler<UpdateBasicProfileCommand, BasicResponse>
{
    private readonly ServerDbContext context;

    public UpdateBasicProfileCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateBasicProfileCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.BasicProfile == null)
        {
            throw new NullReferenceException("Basic Profile is null");
        }

        var cardProfile = context.CardProfiles
            .Include(x => x.PlayerProfile)
            .Include(x => x.CustomizeProfile) 
            .Include(x => x.DefaultTitle)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var basicProfile = updateRequest.BasicProfile;

        cardProfile.UserName = basicProfile.UserName;
        cardProfile.PlayerProfile.OpenEchelon = basicProfile.OpenEchelon;
        cardProfile.PlayerProfile.OpenRecord = basicProfile.OpenRecord;

        UpdateDefaultTitle(cardProfile, basicProfile.DefaultTitle);

        cardProfile.CustomizeProfile.DefaultGaugeDesignId = basicProfile.DefaultGaugeDesignId;
        cardProfile.CustomizeProfile.DefaultBgmPlayMethod = (uint) basicProfile.DefaultBgmPlayingMethod;
        cardProfile.CustomizeProfile.DefaultBgmSettings = String.Join(",", basicProfile.DefaultBgmList.ToArray());
        
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
    
    void UpdateDefaultTitle(CardProfile cardProfile, Title? title)
    {
        if (title is null)
        {
            return;
        }

        cardProfile.DefaultTitle.TitleTextId = title.TextId;
        cardProfile.DefaultTitle.TitleOrnamentId = title.OrnamentId;
        cardProfile.DefaultTitle.TitleEffectId = title.EffectId;
        cardProfile.DefaultTitle.TitleBackgroundPartsId = title.BackgroundPartsId;
    }
}