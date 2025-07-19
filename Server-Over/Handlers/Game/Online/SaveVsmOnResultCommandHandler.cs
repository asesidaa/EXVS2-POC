using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game.Online;

public record SaveVsmOnResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmOnResultCommandHandler : IRequestHandler<SaveVsmOnResultCommand, Response>
{
    private readonly ILogger<SaveVsmOnResultCommandHandler> _logger;
    private readonly ServerDbContext _context;
    
    public SaveVsmOnResultCommandHandler(ILogger<SaveVsmOnResultCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveVsmOnResultCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_on_result = new Response.SaveVsmOnResult()
        });
    }
}
