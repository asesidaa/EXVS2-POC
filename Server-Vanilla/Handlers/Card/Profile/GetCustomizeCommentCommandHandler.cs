using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Handlers.Card.Profile;

public record GetCustomizeCommentCommand(string AccessCode, string ChipId) : IRequest<CustomizeComment>;

public class GetCustomizeCommentCommandHandler : IRequestHandler<GetCustomizeCommentCommand, CustomizeComment>
{
    private readonly ServerDbContext _context;

    public GetCustomizeCommentCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<CustomizeComment> Handle(GetCustomizeCommentCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.CustomizeProfile)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        return Task.FromResult(new CustomizeComment
        {
            BasePhraseId = (int) cardProfile.CustomizeProfile.BasePanelId,
            SubstitutePart1Id = (int) cardProfile.CustomizeProfile.CommentPartsAId,
            SubstitutePart2Id = (int) cardProfile.CustomizeProfile.CommentPartsBId,
        });
    }
}