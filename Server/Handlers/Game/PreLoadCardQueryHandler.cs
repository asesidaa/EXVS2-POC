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
    private readonly ServerDbContext _context;

    public PreLoadCardQueryHandler(ILogger<PreLoadCardQueryHandler> logger, ServerDbContext context)
    {
        this.logger = logger;
        this._context = context;
    }

    public Task<Response> Handle(PreLoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };

        var preLoadCardRequest = request.pre_load_card;

        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == preLoadCardRequest.AccessCode && x.ChipId == preLoadCardRequest.ChipId);

        var sessionId = preLoadCardRequest.AccessCode + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        if (cardProfile != null)
        {
            return ReadAndReturn(preLoadCardRequest, cardProfile, sessionId, response);
        }
        
        logger.LogInformation("Card not exist for ChipId = {}, Now creating...", preLoadCardRequest.ChipId);
        
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

        _context.CardProfiles.Add(newCardProfile);
        _context.SaveChanges();
            
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            IsNewCard = true
        };
        
        return Task.FromResult(response);
    }

    private Task<Response> ReadAndReturn(Request.PreLoadCard preLoadCardRequest, CardProfile cardProfile, string sessionId, Response response)
    {
        logger.LogInformation("Card exists for ChipId = {}, Now reading from Database", preLoadCardRequest.ChipId);
        cardProfile.SessionId = sessionId;
        _context.SaveChanges();

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
            return Task.FromResult(response);
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

        return Task.FromResult(response);
    }
    
    
}