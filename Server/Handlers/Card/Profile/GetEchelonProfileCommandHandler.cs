using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Profile;

public record GetEchelonProfileCommand(String AccessCode, String ChipId) : IRequest<EchelonProfile>;

public class GetEchelonProfileCommandHandler : IRequestHandler<GetEchelonProfileCommand, EchelonProfile>
{
    private readonly ServerDbContext context;

    public GetEchelonProfileCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<EchelonProfile> Handle(GetEchelonProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.PilotDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.LoadPlayer>(cardProfile.PilotDomain.LoadPlayerJson);

        if (user is null)
        {
            throw new NullReferenceException("User is invalid");
        }
        
        return Task.FromResult(new EchelonProfile
        {
            EchelonId = user.EchelonId,
            SpecialEchelonFlag = user.SEchelonFlag,
            EchelonExp = user.EchelonExp,
            AppliedForSpecialEchelonTest = user.SEchelonMissionFlag,
            SpecialEchelonTestProgress = user.SEchelonProgress,
            TotalWin = user.TotalWin,
            TotalLose = user.TotalLose,
            TotalRounds = user.TotalWin + user.TeamLose,
            ShuffleWin = user.ShuffleWin,
            ShuffleLose = user.ShuffleLose,
            ShuffleRounds = user.ShuffleWin + user.ShuffleLose,
            TeamWin = user.TeamWin,
            TeamLose = user.TeamLose,
            TeamRounds = user.TeamWin + user.TeamLose
        });
    }
}