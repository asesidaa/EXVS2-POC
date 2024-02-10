using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerVanilla.Handlers.Card;
using ServerVanilla.Handlers.Card.Gamepad;
using ServerVanilla.Handlers.Card.Message;
using ServerVanilla.Handlers.Card.MobileSuit;
using ServerVanilla.Handlers.Card.Navi;
using ServerVanilla.Handlers.Card.Profile;
using ServerVanilla.Handlers.Card.Team;
using ServerVanilla.Handlers.Card.Triad;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;
using WebUIVanilla.Shared.Validator;

namespace ServerVanilla.Controllers;

[ApiController]
[Route("card")]
public class CardController : BaseController<CardController>
{
    private readonly IMediator _mediator;

    public CardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getAll")]
    [Produces("application/json")]
    public async Task<ActionResult<List<BareboneCardProfile>>> GetAll([FromQuery] GetAllBareboneCardCommand command)
    {
        var response = await _mediator.Send(command);
        return response.Data.ToList();
    }

    [HttpPost("authorize")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> Authorize([FromBody] CardAuthorizationRequest request)
    {
        var response = await _mediator.Send(new PostCardAuthorizationCommand(request));
        return response;
    }
    
    [HttpGet("getBasicDisplayProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicDisplayProfile>> GetBasicDisplayProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetBasicDisplayProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateBasicProfile")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateBasicProfile([FromBody] UpdateBasicProfileRequest request)
    {
        var response = await _mediator.Send(new UpdateBasicProfileCommand(request));
        return response;
    }
    //
    // [HttpGet("getEchelonProfile/{accessCode}/{chipId}")]
    // [Produces("application/json")]
    // public async Task<ActionResult<EchelonProfile>> GetEchelonProfile(String accessCode, String chipId)
    // {
    //     var response = await _mediator.Send(new GetEchelonProfileCommand(accessCode, chipId));
    //     return response;
    // }
    //
    
    [HttpGet("getNaviProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<NaviProfile>> GetNaviProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetNaviProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertDefaultNavi")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertDefaultNavi([FromBody] UpsertDefaultNaviRequest request)
    {
        var response = await _mediator.Send(new UpsertDefaultNaviCommand(request));
        return response;
    }
    
    [HttpPost("upsertNaviCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertNaviCostume([FromBody] UpsertNaviCostumeRequest request)
    {
        var response = await _mediator.Send(new UpsertNaviCostumeCommand(request));
        return response;
    }
    
    [HttpPost("updateAllNaviCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllNaviCostume([FromBody] UpdateAllNaviCostumeRequest request)
    {
        var response = await _mediator.Send(new UpdateAllNaviCostumeCommand(request));
        return response;
    }
    
    [HttpGet("getAllFavouriteMs/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<FavouriteMs>>> GetAllFavouriteMs(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetAllFavouriteMsCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateAllFavouriteMs")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllFavouriteMs([FromBody] UpdateAllFavouriteMsRequest request)
    {
        var response = await _mediator.Send(new UpdateAllFavouriteMsCommand(request));
        return response;
    }
    
    [HttpGet("getUsedMobileSuitData/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<MsSkillGroup>>> GetUsedMobileSuitData(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetUsedMobileSuitDataCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateAllMsCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllMsCostume([FromBody] UpdateAllMsCostumeRequest request)
    {
        var response = await _mediator.Send(new UpdateAllMsCostumeRequestCommand(request));
        return response;
    }
    
    
    [HttpGet("getCpuTriadPartner/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CpuTriadPartner>> GetCpuTriadPartner(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetCpuTriadPartnerCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateCpuTriadPartner")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateCpuTriadPartner(
        [FromBody] UpdateCpuTriadPartnerRequest request)
    {
        bool validateResult = new CpuTriadPartnerValidator().Validate(request.CpuTriadPartner);
    
        if (!validateResult)
        {
            return BadRequest(
                new ErrorResponse
                {
                    ErrorMsg =
                        "Please double check the level... Each of them are maxed at 100, and total is maxed at 500"
                });
        }
    
        var response = await _mediator.Send(new UpdateCpuTriadPartnerCommand(request));
        return response;
    }
    
    [HttpGet("getCustomizeComment/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CustomizeComment>> GetCustomizeComment(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetCustomizeCommentCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateCustomizeComment")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateCustomizeComment(
        [FromBody] UpdateCustomizeCommentRequest request)
    {
        var response = await _mediator.Send(new UpdateCustomizeCommentCommand(request));
        return response;
    }
    
    [HttpGet("getCustomMessageGroupSetting/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CustomMessageGroupSetting>> GetCustomMessageGroupSetting(
        String accessCode,
        String chipId)
    {
        var response = await _mediator.Send(new GetCustomMessageGroupSettingCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertCustomMessages")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertCustomMessages([FromBody] UpsertCustomMessagesRequest request)
    {
        var response = await _mediator.Send(new UpsertCustomMessagesCommand(request));
        return response;
    }
    
    [HttpGet("getGamepadConfig/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<GamepadConfig>> GetGamepadConfig(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetGamepadConfigCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertGamepadConfig")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertGamepadConfig([FromBody] UpsertGamepadConfigRequest request)
    {
        var response = await _mediator.Send(new UpsertGamepadConfigCommand(request));
        return response;
    }
    
    [HttpGet("getTeams/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<Team>>> GetTeams(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetCustomizeTeamCommand(accessCode, chipId));
        return response;
    }
    
    [HttpGet("checkPlayerExistence/{partnerId}/{partnerToken}")]
    [Produces("application/json")]
    public async Task<ActionResult<PlayerExistenceResult>> CheckPlayerExistence(uint partnerId, String partnerToken)
    {
        var response = await _mediator.Send(new CheckPlayerExistenceResultCommand(partnerId, partnerToken));
        return response;
    }
    
    [HttpPost("preCreateTeam")]
    [Produces("application/json")]
    public async Task<ActionResult<PreCreateTeamResponse>> PreCreateTeam([FromBody] PreCreateTeamRequest request)
    {
        var response = await _mediator.Send(new PreCreateTeamCommand(request));
        return response;
    }
    
    [HttpPost("upsertTeams")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertTeams([FromBody] UpsertTeamsRequest request)
    {
        var response = await _mediator.Send(new UpsertCustomizeTeamCommand(request));
        return response;
    }
    
    [HttpGet("getTriadCourseOverallResult/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<TriadCourseOverallResult>> GetTriadCourseResults(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetTriadCourseResultsCommand(accessCode, chipId));
        return response;
    }
    //
    // [HttpGet("getPrivateRoomSetting/{accessCode}/{chipId}")]
    // [Produces("application/json")]
    // public async Task<ActionResult<PrivateRoomSetting>> GetPrivateRoomSetting(String accessCode, String chipId)
    // {
    //     var response = await _mediator.Send(new GetPrivateRoomSettingCommand(accessCode, chipId));
    //     return response;
    // }
    //
    // [HttpPost("updatePrivateRoomSetting")]
    // [Produces("application/json")]
    // public async Task<ActionResult<BasicResponse>> UpdatePrivateRoomSetting([FromBody] PrivateRoomSettingUpdateRequest request)
    // {
    //     var response = await _mediator.Send(new UpdatePrivateRoomSettingCommand(request));
    //     return response;
    // }
}