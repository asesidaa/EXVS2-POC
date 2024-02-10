using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using ServerVanilla.Models.Cards.Replay;
using ServerVanilla.Persistence;

namespace ServerVanilla.Handlers.Game;

public record PreSaveReplayCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class PreSaveReplayCommandHandler : IRequestHandler<PreSaveReplayCommand, Response>
{
    private readonly ILogger<PreSaveReplayCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public PreSaveReplayCommandHandler(ILogger<PreSaveReplayCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task<Response> Handle(PreSaveReplayCommand request, CancellationToken cancellationToken)
    {
        var preSaveRequest = request.Request.pre_save_replay;
        
        _logger.LogInformation("Seemingly an auto upload from LM, best estimating related player...");
        return HandleForAutoUpload(request, preSaveRequest);
    }

    private Task<Response> HandleForAutoUpload(PreSaveReplayCommand request, Request.PreSaveReplay preSaveRequest)
    {
        var filename = "0_" + preSaveRequest.PlayedAt;

        preSaveRequest.Pilots.ForEach(pilot =>
        {
            var cardProfile = _context.CardProfiles
                .Include(x => x.SharedUploadReplays)
                .FirstOrDefault(x => x.Id == pilot.PilotId && x.UserName.ToLower().Contains(pilot.PlayerName.ToLower()));

            if (cardProfile is null)
            {
                _logger.LogInformation("Skip for ({playerId}) {userName}, because seemingly not in this server",
                    pilot.PilotId, pilot.PlayerName);
                return;
            }

            cardProfile.SharedUploadReplays.Add(new SharedUploadReplay()
            {
                Filename = filename,
                ReplaySize = preSaveRequest.ReplaySize,
                PlayedAt = preSaveRequest.PlayedAt,
                StageId = preSaveRequest.StageId,
                PilotsJson = JsonConvert.SerializeObject(preSaveRequest.Pilots),
                SpecialFlag = preSaveRequest.SpecialFlag,
            });

            _context.SaveChanges();
        });

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            pre_save_replay = new Response.PreSaveReplay()
            {
                Url = $"http://{request.BaseAddress}/upload/uploadReplay/0/{preSaveRequest.PlayedAt}"
            }
        });
    }
}