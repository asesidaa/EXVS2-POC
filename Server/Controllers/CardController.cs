using MediatR;
using Microsoft.AspNetCore.Mvc;
using nue.protocol.exvs;
using Server.Handlers.Card;
using Server.Handlers.Card.Gamepad;
using Server.Handlers.Card.Message;
using Server.Handlers.Card.MobileSuit;
using Server.Handlers.Card.Navi;
using Server.Handlers.Card.Profile;
using Server.Handlers.Card.Triad;
using Server.Handlers.Card.UploadedContent;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Validator;

namespace Server.Controllers;

[ApiController]
[Route("card")]
public class CardController : BaseController<CardController>
{
    private readonly IMediator mediator;
    
    public CardController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("getAll")]
    [Produces("application/json")]
    public async Task<ActionResult<List<BareboneCardProfile>>> GetAll()
    {
        var response = await mediator.Send(new GetAllBareboneCardCommand());
        return response;
    }
    
    [HttpGet("getBasicDisplayProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicDisplayProfile>> GetBasicDisplayProfile(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetBasicDisplayProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateBasicProfile")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateBasicProfile([FromBody] UpdateBasicProfileRequest request)
    {
        var response = await mediator.Send(new UpdateBasicProfileCommand(request));
        return response;
    }
    
    [HttpGet("getEchelonProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<EchelonProfile>> GetEchelonProfile(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetEchelonProfileCommand(accessCode, chipId));
        return response;
    }

    
    [HttpGet("getNaviProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<NaviProfile>> GetNaviProfile(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetNaviProfileCommand(accessCode, chipId));
        return response;
    }

    [HttpPost("upsertDefaultNavi")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertDefaultNavi([FromBody] UpsertDefaultNaviRequest request)
    {
        var response = await mediator.Send(new UpsertDefaultNaviCommand(request));
        return response;
    }
    
    [HttpPost("upsertNaviCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertNaviCostume([FromBody] UpsertNaviCostumeRequest request)
    {
        var response = await mediator.Send(new UpsertNaviCostumeCommand(request));
        return response;
    }
    
    [HttpPost("updateAllNaviCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllNaviCostume([FromBody] UpdateAllNaviCostumeRequest request)
    {
        var response = await mediator.Send(new UpdateAllNaviCostumeCommand(request));
        return response;
    }
    
    [HttpGet("getAllFavouriteMs/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<FavouriteMs>>> GetAllFavouriteMs(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetAllFavouriteMsCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateAllFavouriteMs")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllFavouriteMs([FromBody] UpdateAllFavouriteMsRequest request)
    {
        var response = await mediator.Send(new UpdateAllFavouriteMsCommand(request));
        return response;
    }
    
    [HttpGet("getUsedMobileSuitData/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<MsSkillGroup>>> GetUsedMobileSuitData(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetUsedMobileSuitDataCommand(accessCode, chipId));
        return response;
    }

    [HttpPost("updateAllMsCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateAllMsCostume([FromBody] UpdateAllMsCostumeRequest request)
    {
        var response = await mediator.Send(new UpdateAllMsCostumeRequestCommand(request));
        return response;
    }

    [HttpPost("upsertMsCostume")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertMsCostume([FromBody] UpsertMsCostumeRequest request)
    {
        var response = await mediator.Send(new UpsertMsCostumeRequestCommand(request));
        return response;
    }
    
    [HttpGet("getCpuTriadPartner/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CpuTriadPartner>> GetCpuTriadPartner(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetCpuTriadPartnerCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateCpuTriadPartner")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateCpuTriadPartner([FromBody] UpdateCpuTriadPartnerRequest request)
    {
        bool validateResult = new CpuTriadPartnerValidator().Validate(request.CpuTriadPartner);

        if (!validateResult)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorMsg = "Please double check the level... Each of them are maxed at 100, and total is maxed at 500"
            });
        }
        
        var response = await mediator.Send(new UpdateCpuTriadPartnerCommand(request));
        return response;
    }
    
    [HttpGet("getCustomizeComment/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CustomizeComment>> GetCustomizeComment(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetCustomizeCommentCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateCustomizeComment")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateCustomizeComment([FromBody] UpdateCustomizeCommentRequest request)
    {
        var response = await mediator.Send(new UpdateCustomizeCommentCommand(request));
        return response;
    }
    
    [HttpGet("getCustomMessageGroupSetting/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<CustomMessageGroupSetting>> GetCustomMessageGroupSetting(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetCustomMessageGroupSettingCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertCustomMessages")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertCustomMessages([FromBody] UpsertCustomMessagesRequest request)
    {
        var response = await mediator.Send(new UpsertCustomMessagesCommand(request));
        return response;
    }
    
    [HttpGet("getGamepadConfig/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<GamepadConfig>> GetGamepadConfig(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetGamepadConfigCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("upsertGamepadConfig")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertGamepadConfig([FromBody] UpsertGamepadConfigRequest request)
    {
        var response = await mediator.Send(new UpsertGamepadConfigCommand(request));
        return response;
    }
    
    [HttpGet("getUploadedImages/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<List<UploadedImage>>> GetUploadedImages(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetUploadedImagesCommand(accessCode, chipId));
        return response;
    }
    
    [HttpGet("getTriadCourseOverallResult/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<TriadCourseOverallResult>> GetTriadCourseResults(String accessCode, String chipId)
    {
        var response = await mediator.Send(new GetTriadCourseResultsCommand(accessCode, chipId));
        return response;
    }
}