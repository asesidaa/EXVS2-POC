using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Profile;

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

        if (updateRequest.basicProfile == null)
        {
            throw new NullReferenceException("Basic Profile is null");
        }

        var cardProfile = _context.CardProfiles
            .Include(x => x.UserDomain)
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.accessCode && x.ChipId == updateRequest.chipId);

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

        var basicProfile = updateRequest.basicProfile;

        preLoadUser.PlayerName = basicProfile.userName;
        preLoadUser.OpenEchelon = basicProfile.openEchelon;
        preLoadUser.OpenRecord = basicProfile.openRecord;
        mobileUserGroup.Customize.DefaultGaugeDesignId = basicProfile.defaultGaugeDesignId;
        mobileUserGroup.Customize.DefaultBgmPlayMethod = (uint)basicProfile.defaultBgmPlayingMethod;
        mobileUserGroup.Customize.DefaultBgmSettings = basicProfile.defaultBgmList;

        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(preLoadUser);
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUserGroup);
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUserGroup);

        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            success = true
        });
    }
}