using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game.Online;

public record CheckCommunicationQuery(Request Request) : IRequest<Response>;

public class CheckCommunicationQueryHandler : IRequestHandler<CheckCommunicationQuery, Response>
{
    private readonly ILogger<CheckCommunicationQueryHandler> _logger;
    private readonly ServerDbContext _context;
    
    public CheckCommunicationQueryHandler(ILogger<CheckCommunicationQueryHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(CheckCommunicationQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            check_communication = new Response.CheckCommunication()
        });
    }
}