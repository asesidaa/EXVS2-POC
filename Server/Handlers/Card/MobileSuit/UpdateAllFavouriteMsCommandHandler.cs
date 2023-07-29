using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record UpdateAllFavouriteMsCommand(UpdateAllFavouriteMsRequest Request) : IRequest<BasicResponse>;

public class UpdateAllFavouriteMsCommandHandler : IRequestHandler<UpdateAllFavouriteMsCommand, BasicResponse>
{
    private readonly ServerDbContext context;
    
    public UpdateAllFavouriteMsCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateAllFavouriteMsCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.FavouriteMsList.Count > 6)
        {
            throw new InvalidRequestDataException("Favourite MS List should be having maximum length of 6");
        }
        
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        var pilotDataGroup = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);

        if (user == null || pilotDataGroup == null)
        {
            throw new InvalidCardDataException("Card Content is invalid");
        }

        var msSkills = pilotDataGroup.MsSkills;

        var favoriteMsGroups = updateRequest.FavouriteMsList
            .Select(ToFavouriteMsGroup(msSkills))
            .ToList();

        user.FavoriteMobileSuits.Clear();
        user.FavoriteMobileSuits.AddRange(favoriteMsGroups);
        
        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);
        
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Func<FavouriteMs, Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup> ToFavouriteMsGroup(List<Response.LoadCard.PilotDataGroup.MSSkillGroup> msSkills)
    {
        return favouriteMs =>
        {
            var targetMsUsage = msSkills
                .FirstOrDefault(msSkill => msSkill.MstMobileSuitId == favouriteMs.MsId);
            
            return new Response.PreLoadCard.MobileUserGroup.FavoriteMSGroup
            {
                MstMobileSuitId = favouriteMs.MsId,
                MsUsedNum = targetMsUsage?.MsUsedNum ?? 0,
                OpenSkillpoint = true,
                GaugeDesignId = favouriteMs.GaugeDesignId,
                BgmSettings = favouriteMs.BgmList,
                BgmPlayMethod = (uint) favouriteMs.BgmPlayingMethod,
                BattleNavId = favouriteMs.BattleNaviId,
                BurstType = (uint) favouriteMs.BurstType,
                DefaultTitleCustomize = CreateTitleCustomizeTemplate(),
                TriadTitleCustomize = CreateTitleCustomizeTemplate(),
                RankMatchTitleCustomize = CreateTitleCustomizeTemplate()
            };
        };
    }

    TitleCustomize CreateTitleCustomizeTemplate()
    {
        return new TitleCustomize
        {
            TitleTextId = 0,
            TitleOrnamentId = 0,
            TitleEffectId = 0,
            TitleBackgroundPartsId = 0
        };
    }
}