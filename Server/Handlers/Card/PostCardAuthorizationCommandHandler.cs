using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card;

public record PostCardAuthorizationCommand(CardAuthorizationRequest Request) : IRequest<BasicResponse>;

public class PostCardAuthorizationCommandHandler : IRequestHandler<PostCardAuthorizationCommand, BasicResponse>
{
    readonly ILogger _logger;

    readonly ServerDbContext context;

    public PostCardAuthorizationCommandHandler(
        ILogger<PostCardAuthorizationCommandHandler> logger,
        ServerDbContext context)
    {
        _logger = logger;
        this.context = context;
    }

    public async Task<BasicResponse> Handle(PostCardAuthorizationCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = await context.CardProfiles
            .Include(x => x.UserDomain)
            .Where(x => !x.IsNewCard)
            .FirstOrDefaultAsync(x => x.ChipId.Equals(request.Request.ChipId) && x.Id == request.Request.CardId);

        if (cardProfile is not null && request.Request.AccessCode == cardProfile.AccessCode)
            return new BasicResponse { Success = true };

        return new BasicResponse { Success = false };
    }
}
