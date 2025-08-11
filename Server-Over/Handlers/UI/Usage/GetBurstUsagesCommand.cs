using MediatR;
using ServerOver.Mapper.Usage;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;
using WebUIOver.Shared.Dto.Usage;

namespace ServerOver.Handlers.UI.Usage;

public record GetBurstUsagesCommand : IRequest<List<BurstUsageDto>>;

public class GetBurstUsagesCommandHandler: IRequestHandler<GetBurstUsagesCommand, List<BurstUsageDto>>
{
    private readonly ServerDbContext _context;

    public GetBurstUsagesCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<List<BurstUsageDto>> Handle(GetBurstUsagesCommand request, CancellationToken cancellationToken)
    {
        var burstUsages = _context.BurstUsageViews
            .Select(x => x.ToBurstUsageDto())
            .ToList();
        
        foreach (BurstType burstType in Enum.GetValues(typeof(BurstType)))
        {
            var existingBurstUsage = burstUsages.FirstOrDefault(x => (BurstType) x.BurstType == burstType);

            if (existingBurstUsage != null)
            {
                continue;
            }
            
            burstUsages.Add(new BurstUsageDto()
            {
                BurstType = (uint) burstType,
                AggregatedTotalBattle = 0,
                AggregatedTotalWin = 0
            });
        }
        
        burstUsages.Sort((x, y) => x.BurstType.CompareTo(y.BurstType));
        
        return Task.FromResult(burstUsages);
    }
}