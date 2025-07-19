using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Game;

public record LoadTelopQuery(Request Request) : IRequest<Response>;

public class LoadTelopQueryHandler : IRequestHandler<LoadTelopQuery, Response>
{
    private readonly CardServerConfig _config;
    
    public LoadTelopQueryHandler(IOptions<CardServerConfig> options)
    {
        _config = options.Value;
    }
    
    public Task<Response> Handle(LoadTelopQuery request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success
        };

        if (_config.DisableTelop)
        {
            return Task.FromResult(response);
        }

        response.load_telop = new Response.LoadTelop
        {
            TelopData = "Test telop"
        };

        return Task.FromResult(response);
    }
}