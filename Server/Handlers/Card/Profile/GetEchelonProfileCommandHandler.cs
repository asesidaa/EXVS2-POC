using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Profile;

public record GetEchelonProfileCommand(string AccessCode, string ChipId) : IRequest<EchelonProfile>;

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
        
        return Task.FromResult(user.ToEchelonProfile());
    }
}