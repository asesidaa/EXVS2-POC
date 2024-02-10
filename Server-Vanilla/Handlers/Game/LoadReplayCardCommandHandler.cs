using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record LoadReplayCardCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class LoadReplayCardCommandHandler : IRequestHandler<LoadReplayCardCommand, Response>
{
    private readonly ServerDbContext _context;

    public LoadReplayCardCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<Response> Handle(LoadReplayCardCommand request, CancellationToken cancellationToken)
    {
        var loadCardRequest = request.Request.load_replay_card;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.UploadReplays)
            .Include(x => x.SharedUploadReplays)
            .FirstOrDefault(x => x.ChipId == loadCardRequest.ChipId && x.AccessCode == loadCardRequest.AccessCode);

        if (cardProfile is null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                load_replay_card = new Response.LoadReplayCard()
                {
                    AcidError = AcidError.AcidNoUse,
                }
            });
        }

        var loadReplayCard = new Response.LoadReplayCard()
        {
            PilotId = (uint) cardProfile.Id,
            IsNewCard = cardProfile.IsNewCard,
            AccessCode = loadCardRequest.AccessCode,
            User = new Response.LoadReplayCard.MobileUserGroup()
            {
                UserId = (uint) cardProfile.Id,
                PlayerName = cardProfile.UserName
            }
        };
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_replay_card = loadReplayCard
        });
    }
}