using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;

namespace Server.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var readStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"));
        var preLoadCard = JsonConvert.DeserializeObject<Response.PreLoadCard>(readStr);

        if (preLoadCard == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsc_result = new Response.SaveVscResult()
            });
        }
        
        var resultFromRequest = request.Request.save_vsc_result.Result;
        preLoadCard.load_player.EchelonId = resultFromRequest.EchelonId;
        preLoadCard.load_player.EchelonExp += resultFromRequest.EchelonExp;
        
        String outJsonStr = JsonConvert.SerializeObject(preLoadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"), outJsonStr);

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }
}
