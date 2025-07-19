using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.Usage;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Usage;

namespace ServerOver.Controllers.UI.Usage;

[ApiController]
[Route("ui/usage/mobile-suit-stats")]
public class MobileSuitStatisticsController(IMediator mediator, ServerDbContext context) : BaseController<MobileSuitStatisticsController>
{
    [HttpGet("getMobileSuitUsages")]
    [Produces("application/json")]
    public async Task<List<MobileSuitUsageDto>> GetMobileSuitUsages()
    {
        var response = await mediator.Send(new GetMobileSuitUsagesCommand());
        return response;
    }
    
    [HttpGet("getBurstUsages")]
    [Produces("application/json")]
    public async Task<List<BurstUsageDto>> GetBurstUsages()
    {
        var response = await mediator.Send(new GetBurstUsagesCommand());
        return response;
    }
}