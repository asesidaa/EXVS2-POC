using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record PreLoadCardQuery(Request Request) : IRequest<Response>;

public class PreLoadCardQueryHandler : IRequestHandler<PreLoadCardQuery, Response>
{
    private readonly ILogger<PreLoadCardQueryHandler> logger;

    public PreLoadCardQueryHandler(ILogger<PreLoadCardQueryHandler> logger)
    {
        this.logger = logger;
    }

    public Task<Response> Handle(PreLoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };

        logger.LogDebug("Current path {Path}", Directory.GetCurrentDirectory());
        var readStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"));
        response.pre_load_card = JsonConvert.DeserializeObject<Response.PreLoadCard>(readStr);
        
        // String jsonStr = JsonConvert.SerializeObject(response.pre_load_card);
        // Log.Information("JSON String");
        // Log.Information(jsonStr);

        return Task.FromResult(response);
    }
}