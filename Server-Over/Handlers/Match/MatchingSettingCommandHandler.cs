using MediatR;
using nue.protocol.mms;

namespace ServerOver.Handlers.Match;

public record MatchingSettingCommand(Request Request) : IRequest<Response>;

public class MatchingSettingCommandHandler : IRequestHandler<MatchingSettingCommand, Response>
{
    public Task<Response> Handle(MatchingSettingCommand request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Code = ErrorCode.Success
        };

        return Task.FromResult(response);
    }
}