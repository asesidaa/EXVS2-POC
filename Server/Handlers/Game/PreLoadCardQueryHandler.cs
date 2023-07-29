using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Models.Cards;
using Server.Persistence;

namespace Server.Handlers.Game;

public record PreLoadCardQuery(Request Request) : IRequest<Response>;

public class PreLoadCardQueryHandler : IRequestHandler<PreLoadCardQuery, Response>
{
    private readonly ILogger<PreLoadCardQueryHandler> logger;
    private readonly ServerDbContext context;

    public PreLoadCardQueryHandler(ILogger<PreLoadCardQueryHandler> logger, ServerDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public async Task<Response> Handle(PreLoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };

        var preLoadCardRequest = request.pre_load_card;

        var cardProfile = await context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.UserDomain)
            .FirstOrDefaultAsync(x => x.AccessCode == preLoadCardRequest.AccessCode 
                                      && x.ChipId == preLoadCardRequest.ChipId,
                cancellationToken);

        var sessionId = preLoadCardRequest.AccessCode + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        if (cardProfile != null)
        {
            return await ReadAndReturn(preLoadCardRequest, cardProfile, sessionId, response);
        }
        
        logger.LogInformation("Card not exist for ChipId = {ChipId}, Now creating...", preLoadCardRequest.ChipId);
        
        var newCardProfile = new CardProfile
        {
            AccessCode = preLoadCardRequest.AccessCode,
            ChipId = preLoadCardRequest.ChipId,
            SessionId = sessionId,
            PilotDomain = new PilotDomain
            {
                LoadPlayerJson = "{}",
                PilotDataGroupJson = "{}"
            },
            UserDomain = new UserDomain
            {
                UserJson = "{}",
                MobileUserGroupJson = "{}"
            }
        };

        context.CardProfiles.Add(newCardProfile);
        await context.SaveChangesAsync(cancellationToken);
            
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            IsNewCard = true
        };
        
        return response;
    }

    private async Task<Response> ReadAndReturn(Request.PreLoadCard preLoadCardRequest, CardProfile cardProfile, string sessionId, Response response)
    {
        logger.LogInformation("Card exists for ChipId = {ChipId}, Now reading from Database", preLoadCardRequest.ChipId);
        cardProfile.SessionId = sessionId;
        await context.SaveChangesAsync();

        if (cardProfile.IsNewCard)
        {
            logger.LogInformation("ChipId = {} is still a new card, will go to RegisterCard", preLoadCardRequest.ChipId);
            response.pre_load_card = new Response.PreLoadCard
            {
                SessionId = sessionId,
                AcidResponse = null,
                AcidError = AcidError.AcidSuccess,
                IsNewCard = true
            };
            return response;
        }

        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            IsNewCard = false,
            load_player =
                JsonConvert.DeserializeObject<Response.PreLoadCard.LoadPlayer>(cardProfile.PilotDomain.LoadPlayerJson),
            User = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson),
            MatchingTag = null
        };

        return response;
    }
    
    
}