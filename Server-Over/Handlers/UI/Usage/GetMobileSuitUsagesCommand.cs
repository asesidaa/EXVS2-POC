using MediatR;
using ServerOver.Mapper.Usage;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Usage;

namespace ServerOver.Handlers.UI.Usage;

public record GetMobileSuitUsagesCommand : IRequest<List<MobileSuitUsageDto>>;

public class GetMobileSuitUsagesCommandHandler : IRequestHandler<GetMobileSuitUsagesCommand, List<MobileSuitUsageDto>>
{
    private readonly ServerDbContext _context;

    public GetMobileSuitUsagesCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<List<MobileSuitUsageDto>> Handle(GetMobileSuitUsagesCommand request, CancellationToken cancellationToken)
    {
        var mobileSuitUsages = _context.MobileSuitUsageViews
            .Select(mobileSuitUsage => mobileSuitUsage.ToMobileSuitUsageDto())
            .ToList();
        
        return Task.FromResult(mobileSuitUsages);
    }
}