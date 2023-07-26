using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Swan.Collections;

namespace Server.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var readPreLoadCardStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"));
        var preLoadCard = JsonConvert.DeserializeObject<Response.PreLoadCard>(readPreLoadCardStr);

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
        
        var readLoadCardStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"));
        var loadCard = JsonConvert.DeserializeObject<Response.LoadCard>(readLoadCardStr);
        
        if (loadCard == null)
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
        
        var cpuSceneData = loadCard.pilot_data_group.CpuScenes
            .FirstOrDefault(data => data.CourseId == resultFromRequest.CourseId);

        if (cpuSceneData == null)
        {
            loadCard.pilot_data_group.CpuScenes.Add(new Response.LoadCard.PilotDataGroup.CpuSceneData
            {
                CourseId = resultFromRequest.CourseId,
                ReleasedAt = 0,
                TotalPlayNum = 0,
                TotalClearNum = 0,
                Highscore = 0
            });
        }
        else
        {
            if (resultFromRequest.CourseScore != null)
            {
                if (resultFromRequest.CourseScore.GetValueOrDefault(0) >= cpuSceneData.Highscore)
                {
                    cpuSceneData.Highscore = resultFromRequest.CourseScore.GetValueOrDefault(0);
                }
                cpuSceneData.TotalPlayNum += 1;
                cpuSceneData.TotalClearNum += 1;
            }
        }

        if (resultFromRequest.ReleasedRibbonIds != null)
        {
            loadCard.pilot_data_group.CpuRibbons = loadCard.pilot_data_group.CpuRibbons
                .Concat(resultFromRequest.ReleasedRibbonIds)
                .Distinct()
                .ToArray();
        }

        String outPreLoadCardJsonStr = JsonConvert.SerializeObject(preLoadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "preloadcard.json"), outPreLoadCardJsonStr);
            
        String outLoadCardJsonStr = JsonConvert.SerializeObject(loadCard);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "loadcard.json"),outLoadCardJsonStr);

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }
}
