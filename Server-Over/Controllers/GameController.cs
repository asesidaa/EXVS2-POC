using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using nue.protocol.exvs;
using ServerOver.Handlers.Game;
using ServerOver.Handlers.Game.Online;
using ServerOver.Handlers.Game.Tournament;
using ServerOver.Persistence;
using Swan.Formatters;

namespace ServerOver.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class GameController : BaseController<GameController>
{
    private readonly IMediator mediator;
    private readonly ServerDbContext context;
    
    public GameController(IMediator mediator, ServerDbContext context)
    {
        this.mediator = mediator;
        this.context = context;
    }
    
    [Route("")]
    [HttpPost]
    [Produces("application/protobuf")]
    public async Task<IActionResult> Game([FromBody] Request request)
    {
        Logger.LogInformation("Request is {Request}", request.Stringify());
        
        var baseAddress = Request.Host.ToString();
        
        var response = request.Type switch
        {
            MethodType.MthdRegisterPcb => await mediator.Send(new RegisterPcbCommand(request, baseAddress)),
            MethodType.MthdRegisterPcbAck => await mediator.Send(new RegisterPcbAckCommand(request)),
            MethodType.MthdSaveInsideData => await mediator.Send(new SaveInsideDataCommand(request)),
            MethodType.MthdLoadGameData => await mediator.Send(new LoadGameDataQuery(request)),
            MethodType.MthdLoadRankMatch => await mediator.Send(new LoadRankMatchQuery(request)),
            MethodType.MthdLoadClassMatch => await mediator.Send(new LoadClassMatchCommand(request)),
            MethodType.MthdPreLoadCard => await mediator.Send(new PreLoadCardQuery(request)),
            MethodType.MthdLoadCard => await mediator.Send(new LoadCardQuery(request)),
            MethodType.MthdRegisterCard => await mediator.Send(new RegisterCardCommand(request)),
            MethodType.MthdSaveVsmResult => await mediator.Send(new SaveVsmResultCommand(request)),
            MethodType.MthdSaveVsmOnResult => await mediator.Send(new SaveVsmOnResultCommand(request)),
            MethodType.MthdSaveVscResult => await mediator.Send(new SaveVscResultCommand(request)),
            MethodType.MthdSaveVstResult => await mediator.Send(new SaveVstResultCommand(request)),
            MethodType.MthdSaveCharge => await mediator.Send(new SaveChargeCommand(request)),
                
            MethodType.MthdSaveBattleLog => await mediator.Send(new SaveBattleLogCommand(request)),
            MethodType.MthdSaveBattleLogOn => await mediator.Send(new SaveBattleLogOnCommand(request)),
            MethodType.MthdSaveUserPlayResearchData => await mediator.Send(new SaveUserPlayResearchDataCommand(request)),
            MethodType.MthdSaveResultCapture => await mediator.Send(new SaveResultCaptureCommand(request, baseAddress)),
            
            MethodType.MthdCheckCommunication => await mediator.Send(new CheckCommunicationQuery(request)),
            
            MethodType.MthdLoadSpotInfo => await mediator.Send(new LoadSpotInfoCommand(request)),
            MethodType.MthdLoadRanking => await mediator.Send(new LoadRankingQuery(request)),
            MethodType.MthdCheckTelop => await mediator.Send(new CheckTelopQuery(request)),
            MethodType.MthdLoadTelop => await mediator.Send(new LoadTelopQuery(request)),
            MethodType.MthdCheckMovieRelease => await mediator.Send(new CheckMovieReleaseQuery(request)),
            MethodType.MthdLoadSpotUrl => await mediator.Send(new LoadSpotUrlQuery(request)),
            // MethodType.MthdLoadReplayCard => await mediator.Send(new LoadReplayCardCommand(request, baseAddress)),
            // MethodType.MthdPreSaveReplay => await mediator.Send(new PreSaveReplayCommand(request, baseAddress)),
            MethodType.MthdLoadMeetingCard => await mediator.Send(new LoadMeetingCardCommand(request)),
            MethodType.MthdSaveTournamentResult => await mediator.Send(new SaveTournamentResultCommand(request)),
            MethodType.MthdLoadBlackList => await mediator.Send(new LoadBlackListQuery(request)),
            MethodType.MthdStartTournament => await mediator.Send(new StartTournamentCommand(request)),
            MethodType.MthdPing => await mediator.Send(new PingCommand(request)),
            MethodType.MthdCheckTime => await mediator.Send(new CheckTimeQuery(request)),
            MethodType.MthdSaveLog => await mediator.Send(new SaveLogCommand(request)),
            MethodType.MthdCheckResourceData => await mediator.Send(new CheckResourceDataQuery(request)),
            _ => UnhandledResponse(request)
        };

        return Ok(response);
    }
    
    private Response UnhandledResponse(Request request)
    {
        Logger.LogWarning("Unhandled case: {Type}", request.Type);
        return new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.ErrServer,
            ErrorMsg = "Unhandled case"
        };
    }
}