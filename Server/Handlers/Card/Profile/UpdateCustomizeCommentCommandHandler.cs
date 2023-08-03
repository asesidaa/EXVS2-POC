using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Profile;

public record UpdateCustomizeCommentCommand(UpdateCustomizeCommentRequest Request) : IRequest<BasicResponse>;

public class UpdateCustomizeCommentCommandHandler : IRequestHandler<UpdateCustomizeCommentCommand, BasicResponse>
{
    private readonly ServerDbContext context;

    public UpdateCustomizeCommentCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateCustomizeCommentCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var mobileUser = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);

        if (mobileUser is null)
        {
            throw new NullReferenceException("User is invalid");
        }

        mobileUser.Customize.BasePanelId = (uint) updateRequest.CustomizeComment.BasePhraseId;
        mobileUser.Customize.CommentPartsAId = (uint) updateRequest.CustomizeComment.SubstitutePart1Id;
        mobileUser.Customize.CommentPartsBId = (uint) updateRequest.CustomizeComment.SubstitutePart2Id;

        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUser);

        context.SaveChanges();
        
        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}