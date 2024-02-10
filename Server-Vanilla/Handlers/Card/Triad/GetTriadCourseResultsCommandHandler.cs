using MediatR;
using ServerVanilla.Mapper.Card.Triad;
using ServerVanilla.Persistence;
using ServerVanilla.Utils;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Exception;

namespace ServerVanilla.Handlers.Card.Triad;

public record GetTriadCourseResultsCommand(string AccessCode, string ChipId) : IRequest<TriadCourseOverallResult>;

public class GetTriadCourseResultsCommandHandler : IRequestHandler<GetTriadCourseResultsCommand, TriadCourseOverallResult>
{
    private readonly ServerDbContext _context;

    public GetTriadCourseResultsCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<TriadCourseOverallResult> Handle(GetTriadCourseResultsCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var triadCourseDatas = _context.TriadCourseDataDbSet
            .Where(x => x.CardProfile == cardProfile)
            .Select(cpuScenes => cpuScenes.ToTriadCourseResult())
            .ToList();

        var triadMiscInfo = _context.TriadMiscInfoDbSet
            .First(x => x.CardProfile == cardProfile);

        var triadCourseOverallResult = new TriadCourseOverallResult
        {
            TriadCourseResults = triadCourseDatas,
            CpuRibbons = ArrayUtil.FromString(triadMiscInfo.CpuRibbons)
        };
        
        return Task.FromResult(triadCourseOverallResult);
    }
}