using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Serilog;

namespace Server.Handlers.Game;

public record LoadCardQuery(Request Request) : IRequest<Response>;

public class LoadCardQueryHandler : IRequestHandler<LoadCardQuery, Response>
{
    public Task<Response> Handle(LoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
        };
        
        String readStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"));
        response.load_card = JsonConvert.DeserializeObject<Response.LoadCard>(readStr);
        
        return Task.FromResult(response);
    }
}