using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveChargeCommand(Request Request) : IRequest<Response>;

public class SaveChargeCommandHandler : IRequestHandler<SaveChargeCommand, Response>
{
    public Task<Response> Handle(SaveChargeCommand request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
        };
        response.save_charge = new Response.SaveCharge();
        foreach (var unused in request.Request.save_charge.ChargeDatas)
        {
            response.save_charge.SaveChargeResults.Add(new Response.SaveCharge.SaveChargeResult
            {
                hc2_error = nue.protocol.exvs.Response.SaveCharge.SaveChargeResult.Hc2Error.Hc2Success
            });
        }

        return Task.FromResult(response);
    }
}