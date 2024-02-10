using MediatR;
using nue.protocol.exvs;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record RegisterCardCommand(Request Request) : IRequest<Response>;

public class RegisterCardCommandHandler : IRequestHandler<RegisterCardCommand, Response>
{
    private readonly ILogger<RegisterCardCommandHandler> _logger;
    private readonly ServerDbContext _context;
    
    public RegisterCardCommandHandler(ILogger<RegisterCardCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(RegisterCardCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var registerCardRequest = request.register_card;
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == registerCardRequest.AccessCode && x.SessionId == registerCardRequest.SessionId);

        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }

        var pilotId = cardProfile.Id;
        
        cardProfile.IsNewCard = false;
        
        _context.SaveChanges();
        
        response.register_card = new Response.RegisterCard
        {
            SessionId = registerCardRequest.SessionId,
            AccessCode = registerCardRequest.AccessCode,
            AcidError = AcidError.AcidNoUse,
            IsRegistered = true,
            PilotId = Convert.ToUInt32(pilotId)
        };
        
        return Task.FromResult(response);
    }
}