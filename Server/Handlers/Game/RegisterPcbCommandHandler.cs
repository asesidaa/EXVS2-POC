using MediatR;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record RegisterPcbCommand(Request Request) : IRequest<Response>;

public class RegisterPcbCommandHandler : IRequestHandler<RegisterPcbCommand, Response>
{
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
                Ipv4Flag = true,
                NextMaintenanceStartAt = 2005364002,
                NextMaintenanceEndAt = 2005364004,
                SramClear = true,
                // LmIpAddresses = {"192.168.50.239"},
                ServerInfoes =
                {
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvMatch,
                        Uri = "vsapi.taiko-p.jp/match",
                        Port = 12345
                    }
                },
                core_dump_res =
                {
                    new Response.RegisterPcb.CoreDumpRes
                    {
                        FileName = "test",
                        Url = "http://vsapi.taiko-p.jp/test"
                    }
                }
            }
        };

        return Task.FromResult(response);
    }
}