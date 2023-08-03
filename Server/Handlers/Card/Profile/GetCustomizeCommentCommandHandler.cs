using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Profile;

public record GetCustomizeCommentCommand(string AccessCode, string ChipId) : IRequest<CustomizeComment>;

public class GetCustomizeCommentCommandHandler : IRequestHandler<GetCustomizeCommentCommand, CustomizeComment>
{
    private readonly ServerDbContext context;

    public GetCustomizeCommentCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<CustomizeComment> Handle(GetCustomizeCommentCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var mobileUser = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);

        if (mobileUser is null)
        {
            throw new NullReferenceException("User is invalid");
        }
        
        return Task.FromResult(new CustomizeComment
        {
            BasePhraseId = (int) mobileUser.Customize.BasePanelId,
            SubstitutePart1Id = (int) mobileUser.Customize.CommentPartsAId,
            SubstitutePart2Id = (int) mobileUser.Customize.CommentPartsBId,
        });
    }
}