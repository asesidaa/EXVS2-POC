using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using nue.protocol.exvs;
using Server.Handlers.Game;
using Swan.Formatters;

namespace Server.Controllers;

public class GameController : BaseController<GameController>
{
    private readonly IMediator mediator;

    public GameController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("")]
    [HttpPost]
    [Produces("application/protobuf")]
    public async Task<IActionResult> Game([FromBody] Request request)
    {
        Logger.LogInformation("Request is {Request}", request.Stringify());
        var response = request.Type switch
        {
            MethodType.MthdRegisterPcb          => await mediator.Send(new RegisterPcbCommand(request)),
            MethodType.MthdRegisterPcbAck       => await mediator.Send(new RegisterPcbAckCommand(request)),
            MethodType.MthdSaveInsideData       => await mediator.Send(new SaveInsideDataCommand(request)),
            MethodType.MthdPing                 => await mediator.Send(new PingCommand(request)),
            MethodType.MthdCheckTime            => await mediator.Send(new CheckTimeQuery(request)),
            MethodType.MthdCheckResourceData    => await mediator.Send(new CheckResourceDataQuery(request)),
            MethodType.MthdLoadGameData         => await mediator.Send(new LoadGameDataQuery(request)),
            MethodType.MthdLoadRankMatch        => await mediator.Send(new LoadRankMatchQuery(request)),
            MethodType.MthdSaveLog              => await mediator.Send(new SaveLogCommand(request)),
            MethodType.MthdCheckCommunication   => await mediator.Send(new CheckCommunicationQuery(request)),
            MethodType.MthdLoadBlackList        => await mediator.Send(new LoadBlackListQuery(request)),
            MethodType.MthdSaveTournamentResult => await mediator.Send(new SaveTournamentResultCommand(request)),
            MethodType.MthdLoadRanking          => await mediator.Send(new LoadRankingQuery(request)),
            MethodType.MthdCheckTelop           => await mediator.Send(new CheckTelopQuery(request)),
            MethodType.MthdLoadTelop            => await mediator.Send(new LoadTelopQuery(request)),
            MethodType.MthdCheckMovieRelease    => await mediator.Send(new CheckMovieReleaseQuery(request)),
            MethodType.MthdLoadSpotUrl          => await mediator.Send(new LoadSpotUrlQuery(request)),
            MethodType.MthdSaveBattleLog        => await mediator.Send(new SaveBattleLogCommand(request)),
            MethodType.MthdSaveBattleLogOn      => await mediator.Send(new SaveBattleLogOnCommand(request)),
            MethodType.MthdSaveVsmResult        => await mediator.Send(new SaveVsmResultCommand(request)),
            MethodType.MthdSaveVsmOnResult      => await mediator.Send(new SaveVsmOnResultCommand(request)),
            MethodType.MthdSaveVscResult        => await mediator.Send(new SaveVscResultCommand(request)),
            MethodType.MthdSaveVstResult        => await mediator.Send(new SaveVstResultCommand(request)),
            MethodType.MthdSaveVsmToResult      => await mediator.Send(new SaveVsmToResultCommand(request)),
            MethodType.MthdSaveCharge           => await mediator.Send(new SaveChargeCommand(request)),
            MethodType.MthdSaveUserPlayResearchData =>
                await mediator.Send(new SaveUserPlayResearchDataCommand(request)),
            MethodType.MthdPreLoadCard          => await mediator.Send(new PreLoadCardQuery(request)),
            MethodType.MthdRegisterCard         => await mediator.Send(new RegisterCardCommand(request)),
            MethodType.MthdLoadCard             => await mediator.Send(new LoadCardQuery(request)),
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

    [Route("")]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello");
    }
}