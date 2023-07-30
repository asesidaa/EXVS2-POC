using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Profile;

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
            .Include(x => x.UserDomain)
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var preLoadUser =
            JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        var mobileUserGroup =
            JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain
                .MobileUserGroupJson);
        
        if (preLoadUser == null || mobileUserGroup == null)
        {
            throw new NullReferenceException("Card Content is invalid");
        }

        var basicProfile = updateRequest.BasicProfile;

        preLoadUser.PlayerName = basicProfile.UserName;
        preLoadUser.OpenEchelon = basicProfile.OpenEchelon;
        preLoadUser.OpenRecord = basicProfile.OpenRecord;
        preLoadUser.customize_group.DefaultTitleCustomize = CreateTitleCustomize(basicProfile.DefaultTitle);

        mobileUserGroup.Customize.DefaultGaugeDesignId = basicProfile.DefaultGaugeDesignId;
        mobileUserGroup.Customize.DefaultBgmPlayMethod = (uint)basicProfile.DefaultBgmPlayingMethod;
        mobileUserGroup.Customize.DefaultBgmSettings = basicProfile.DefaultBgmList;
        mobileUserGroup.TriadTitleCustomize = CreateTitleCustomize(basicProfile.TriadTitle);
        mobileUserGroup.RankMatchTitleCustomize = CreateTitleCustomize(basicProfile.RankingTitle);

        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(preLoadUser);
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUserGroup);

        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
    
    TitleCustomize CreateTitleCustomize(Title? title)
    {
        if (title is null)
        {
            return new TitleCustomize
            {
                TitleTextId = 0,
                TitleOrnamentId = 0,
                TitleEffectId = 0,
                TitleBackgroundPartsId = 0
            };
        }
        
        return new TitleCustomize
        {
            TitleTextId = title.TextId,
            TitleOrnamentId = title.OrnamentId,
            TitleEffectId = title.EffectId,
            TitleBackgroundPartsId = title.BackgroundPartsId
        };
    }
}