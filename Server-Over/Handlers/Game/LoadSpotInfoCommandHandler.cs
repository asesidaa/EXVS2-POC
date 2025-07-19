using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Game;

public record LoadSpotInfoCommand(Request Request) : IRequest<Response>;

public class LoadSpotInfoCommandHandler : IRequestHandler<LoadSpotInfoCommand, Response>
{
    private readonly CardServerConfig _config;
    
    public LoadSpotInfoCommandHandler(IOptions<CardServerConfig> options)
    {
        _config = options.Value;
    }
    
    public Task<Response> Handle(LoadSpotInfoCommand request, CancellationToken cancellationToken)
    {
        var loadSpotInfoResponse = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
        };

        if (_config.DisableTelop)
        {
            return Task.FromResult(loadSpotInfoResponse);
        }
        
        loadSpotInfoResponse.load_spot_info = new Response.LoadSpotInfo()
        {
            SinfoBgid = 1,
            SinfoTitle = "Spot Title",
            SinfoText = "Spot Text",
            InfbarSwitch = true
        };
        
        return Task.FromResult(loadSpotInfoResponse);
    }
}