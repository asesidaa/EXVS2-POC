using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveChargeCommand(Request Request) : IRequest<Response>;

public class SaveChargeCommandHandler : IRequestHandler<SaveChargeCommand, Response>
{
    private readonly ServerDbContext _context;

    public SaveChargeCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<Response> Handle(SaveChargeCommand request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
        };
        
        return Task.FromResult(response);
    }
}