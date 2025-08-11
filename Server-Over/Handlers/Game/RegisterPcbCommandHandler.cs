using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record RegisterPcbCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class RegisterPcbCommandHandler : IRequestHandler<RegisterPcbCommand, Response>
{
    private readonly CardServerConfig _config;
    private readonly ServerDbContext _context;
    
    public RegisterPcbCommandHandler(IOptions<CardServerConfig> options, ServerDbContext context)
    {
        _config = options.Value;
        _context = context;
    }
    
    public Task<Response> Handle(RegisterPcbCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            register_pcb = new Response.RegisterPcb
            {
                NextMaintenanceStartAt = 2005364002,
                NextMaintenanceEndAt = 2005364004,
                SramClear = true,
                // LmIpAddresses = {"192.168.50.239"},
                Ipv4Flag = true,
                ServerInfoes =
                {
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvMatch,
                        Uri = $"{command.BaseAddress}/match",
                        Port = 12345
                    }
                }
            }
        };

        return Task.FromResult(response);
    }
}