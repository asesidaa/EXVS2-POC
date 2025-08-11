using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Card;

public record GetByAccessCodeCommand(String AccessCode) : IRequest<BareboneCardProfile>;

public class GetByAccessCodeCommandHandler : IRequestHandler<GetByAccessCodeCommand, BareboneCardProfile>
{
    private readonly ILogger<GetByAccessCodeCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public GetByAccessCodeCommandHandler(ILogger<GetByAccessCodeCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<BareboneCardProfile> Handle(GetByAccessCodeCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => !x.IsNewCard && x.AccessCode == request.AccessCode);

        if (cardProfile == null)
        {
            var stubCardProfile = new BareboneCardProfile
            {
                CardId = 0,
                ChipId = "",
                UserName = ""
            };
            
            return Task.FromResult(stubCardProfile);
        }
        
        var bareboneCardProfile = new BareboneCardProfile
        {
            CardId = cardProfile.Id,
            ChipId = cardProfile.ChipId,
            UserName = cardProfile.UserName
        };
        
        return Task.FromResult(bareboneCardProfile);
    }
}