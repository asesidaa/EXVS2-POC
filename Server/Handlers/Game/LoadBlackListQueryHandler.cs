using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record LoadBlackListQuery(Request Request) : IRequest<Response>;

public class LoadBlackListQueryHandler : IRequestHandler<LoadBlackListQuery, Response>
{
    public Task<Response> Handle(LoadBlackListQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_black_list = new Response.LoadBlackList
            {
                ThresholdDelayedRtt = 300,
                DelayPermitCondition = 300,
                DelayRestrictCondition = 300,
                MaxBlacklistNum = 1,
                ThresholdDelayedFrame = 5
            }
        });
    }
}