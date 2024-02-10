using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Models.Cards;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.Profile;

public record UpdateCustomizeCommentCommand(UpdateCustomizeCommentRequest Request) : IRequest<BasicResponse>;

public class UpdateCustomizeCommentCommandHandler : IRequestHandler<UpdateCustomizeCommentCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpdateCustomizeCommentCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateCustomizeCommentCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = Queryable
            .FirstOrDefault<CardProfile>(_context.CardProfiles
                .Include(x => x.CustomizeProfile), x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        cardProfile.CustomizeProfile.BasePanelId = (uint) updateRequest.CustomizeComment.BasePhraseId;
        cardProfile.CustomizeProfile.CommentPartsAId = (uint) updateRequest.CustomizeComment.SubstitutePart1Id;
        cardProfile.CustomizeProfile.CommentPartsBId = (uint) updateRequest.CustomizeComment.SubstitutePart2Id;
        
        _context.SaveChanges();
        
        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}