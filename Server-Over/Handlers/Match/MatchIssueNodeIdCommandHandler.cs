using MediatR;
using Microsoft.Extensions.Caching.Memory;
using nue.protocol.mms;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Match;

public record MatchIssueNodeIdCommand(Request Request) : IRequest<Response>;

public class MatchIssueNodeIdCommandHandler : IRequestHandler<MatchIssueNodeIdCommand, Response>
{
    private readonly IMemoryCache _memoryCache;
    private readonly ServerDbContext _context;
    
    public MatchIssueNodeIdCommandHandler(IMemoryCache memoryCache, ServerDbContext context)
    {
        _memoryCache = memoryCache;
        _context = context;
    }
    
    public Task<Response> Handle(MatchIssueNodeIdCommand request, CancellationToken cancellationToken)
    {
        var nodeList = new List<uint> { 1 };
        
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Code = ErrorCode.Success,
            issue_node_id = new Response.IssueNodeId
            {
                NodeIds = nodeList.ToArray()
            }
        };

        return Task.FromResult(response);
    }
}