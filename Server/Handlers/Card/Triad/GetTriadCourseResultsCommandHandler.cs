using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Triad;

public record GetTriadCourseResultsCommand(string AccessCode, string ChipId) : IRequest<TriadCourseOverallResult>;

public class GetTriadCourseResultsCommandHandler : IRequestHandler<GetTriadCourseResultsCommand, TriadCourseOverallResult>
{
    private readonly ServerDbContext context;

    public GetTriadCourseResultsCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<TriadCourseOverallResult> Handle(GetTriadCourseResultsCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var pilotDataGroup = JsonConvert.DeserializeObject<Response.LoadCard.PilotDataGroup>(cardProfile.PilotDomain.PilotDataGroupJson);

        if (pilotDataGroup is null)
        {
            throw new NullReferenceException("User is invalid");
        }

        var triadCourseResults = pilotDataGroup.CpuScenes
            .Select(cpuScenes => cpuScenes.ToTriadCourseResult())
            .ToList();

        var triadCourseOverallResult = new TriadCourseOverallResult
        {
            TriadCourseResults = triadCourseResults,
            CpuRibbons = pilotDataGroup.CpuRibbons
        };
        
        return Task.FromResult(triadCourseOverallResult);
    }
}