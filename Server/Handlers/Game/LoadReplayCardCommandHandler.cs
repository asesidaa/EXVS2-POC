using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Models.Config;
using Server.Persistence;

namespace Server.Handlers.Game;

public record LoadReplayCardCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class LoadReplayCardCommandHandler : IRequestHandler<LoadReplayCardCommand, Response>
{
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;

    public LoadReplayCardCommandHandler(ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _context = context;
        _config = options.Value;
    }

    public Task<Response> Handle(LoadReplayCardCommand request, CancellationToken cancellationToken)
    {
        var loadCardRequest = request.Request.load_replay_card;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.UploadReplays)
            .FirstOrDefault(x => x.ChipId == loadCardRequest.ChipId && x.AccessCode == loadCardRequest.AccessCode);

        if (cardProfile is null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                load_replay_card = new Response.LoadReplayCard()
                {
                    PilotId = null,
                    IsNewCard = null,
                    AcidError = AcidError.AcidNoUse,
                }
            });
        }

        var loadReplayCard = new Response.LoadReplayCard()
        {
            PilotId = (uint) cardProfile.Id,
            IsNewCard = cardProfile.IsNewCard,
            User = new Response.LoadReplayCard.MobileUserGroup()
            {
                UserId = (uint) cardProfile.Id,
                Paid = true,
                SlotNum = _config.MaxReplaySaveSlotPerPlayer - (uint) cardProfile.UploadReplays.Count,
                TicketNum = 99,
                replay_config = new Response.LoadReplayCard.MobileUserGroup.ReplayConfig()
                {
                    View1 = 1,
                    Timeline = true
                }
            }
        };
        
        cardProfile.UploadReplays
            .ToList()
            .ForEach(replay =>
            {
                var rawPilots = JsonConvert.DeserializeObject<List<Request.PreSaveReplay.PilotGroup>>(replay.PilotsJson);

                var replayInfo = new Response.LoadReplayCard.MobileUserGroup.ReplayInfo()
                {
                    Url = $"http://{request.BaseAddress}/replay/{replay.Filename}.json",
                    ReplayType = 0, // 1 = Friend
                    PlayedAt = replay.PlayedAt,
                    ReplaySize = replay.ReplaySize
                };

                if (rawPilots == null)
                {
                    return;
                }
                
                rawPilots.ForEach(pilot =>
                {
                    replayInfo.Pilots.Add(new Response.LoadReplayCard.MobileUserGroup.ReplayInfo.PilotGroup()
                    {
                        MstMobileSuitId = pilot.MstMobileSuitId,
                        PilotId = pilot.PilotId,
                        PlayerName = pilot.PlayerName
                    });
                });
                
                loadReplayCard.User.ReplayServices.Add(replayInfo);
            });

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_replay_card = loadReplayCard
        });
    }
}