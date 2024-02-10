using MediatR;
using nue.protocol.exvs;

namespace ServerVanilla.Handlers.Game;

public record RegisterPcbCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class RegisterPcbCommandHandler : IRequestHandler<RegisterPcbCommand, Response>
{
    // private readonly CardServerConfig _config;
    
    // public RegisterPcbCommandHandler(IOptions<CardServerConfig> options)
    // {
    //     _config = options.Value;
    // }
    
    public RegisterPcbCommandHandler()
    {
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
                ServerInfoes =
                {
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvMatch,
                        Uri = $"{command.BaseAddress}/match",
                        Port = 12345
                    },
                    // new Response.RegisterPcb.ServerInfo
                    // {
                    //     ServerType = ServerType.SrvStun,
                    //     Uri = _config.StunServerConfig.Host,
                    //     Port = _config.StunServerConfig.Port,
                    //     AccountName = _config.StunServerConfig.Username,
                    //     AccountPass = _config.StunServerConfig.Password
                    // }
                }
            }
        };

        return Task.FromResult(response);
    }
}