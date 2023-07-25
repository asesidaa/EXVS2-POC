using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record UsePCoinTicketCommand(Request Request) : IRequest<Response>;

public class UsePCoinTicketCommandHandler : IRequestHandler<UsePCoinTicketCommand, Response>
{
    public Task<Response> Handle(UsePCoinTicketCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            use_p_coin_ticket = new Response.UsePCoinTicket
            {
                Approve = true
            }
        });
    }
}
