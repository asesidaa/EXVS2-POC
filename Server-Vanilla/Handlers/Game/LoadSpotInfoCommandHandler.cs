using MediatR;
using nue.protocol.exvs;

namespace ServerVanilla.Handlers.Game;

public record LoadSpotInfoCommand(Request Request) : IRequest<Response>;

public class LoadSpotInfoCommandHandler : IRequestHandler<LoadSpotInfoCommand, Response>
{
    public Task<Response> Handle(LoadSpotInfoCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            // load_spot_info = new Response.LoadSpotInfo()
            // {
            //     SinfoBgid = 1,
            //     SinfoTitle = "Spot Title",
            //     SinfoText = "Spot Text",
            //     InfbarSwitch = true
            // }
        });
    }
}