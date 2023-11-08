using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Models.Cards;
using Server.Models.Config;
using Server.Persistence;

namespace Server.Handlers.Game;

public record PreSaveReplayCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class PreSaveReplayCommandHandler : IRequestHandler<PreSaveReplayCommand, Response>
{
    private readonly ILogger<PreSaveReplayCommandHandler> _logger;
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;

    public PreSaveReplayCommandHandler(ILogger<PreSaveReplayCommandHandler> logger, ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _logger = logger;
        _context = context;
        _config = options.Value;
    }

    public Task<Response> Handle(PreSaveReplayCommand request, CancellationToken cancellationToken)
    {
        var preSaveRequest = request.Request.pre_save_replay;

        if (preSaveRequest.MobileUserId == 0)
        {
            _logger.LogInformation("Seemingly an auto upload from LM, best estimating related player...");
            return HandleForAutoUpload(request, preSaveRequest);
        }

        return HandleForPlayerUpload(request, preSaveRequest);
    }

    private Task<Response> HandleForAutoUpload(PreSaveReplayCommand request, Request.PreSaveReplay preSaveRequest)
    {
        var filename = preSaveRequest.MobileUserId + "_" + preSaveRequest.PlayedAt;

        preSaveRequest.Pilots.ForEach(pilot =>
        {
            var userNameJson = $"\"PlayerName\":\"{pilot.PlayerName.ToLower()}";
            userNameJson = userNameJson.ToLower();

            var cardProfile = _context.CardProfiles
                .Include(x => x.SharedUploadReplays)
                .Include(x => x.UserDomain)
                .FirstOrDefault(x => x.Id == pilot.PilotId && x.UserDomain.UserJson.ToLower().Contains(userNameJson));

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
                ReplayServiceFlag = preSaveRequest.ReplayServiceFlag,
                MobileUserId = preSaveRequest.MobileUserId,
                MatchingMode = preSaveRequest.MatchingMode,
                TeamType = preSaveRequest.TeamType,
                ReturnMatchFlag = preSaveRequest.ReturnMatchFlag,
                TournamentId = preSaveRequest.TournamentId
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
                Status = 1,
                Url = $"http://{request.BaseAddress}/upload/uploadReplay/{preSaveRequest.MobileUserId}/{preSaveRequest.PlayedAt}"
            }
        });
    }

    private Task<Response> HandleForPlayerUpload(PreSaveReplayCommand request, Request.PreSaveReplay preSaveRequest)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.UploadReplays)
            .FirstOrDefault(x => x.Id == preSaveRequest.MobileUserId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        if (cardProfile.UploadReplays.Count == _config.MaxReplaySaveSlotPerPlayer)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                pre_save_replay = new Response.PreSaveReplay()
                {
                    Status = 2, // 0 = Fail, 2 = Reject
                    Url = "http://dummy"
                }
            });
        }

        var existingReplay = cardProfile.UploadReplays
            .FirstOrDefault(replay =>
                replay.MobileUserId == preSaveRequest.MobileUserId && replay.ReplaySize == preSaveRequest.ReplaySize &&
                replay.PlayedAt == preSaveRequest.PlayedAt);

        if (existingReplay is not null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                pre_save_replay = new Response.PreSaveReplay()
                {
                    Status = 2,
                    Url = "http://{request.BaseAddress}/upload/uploadReplay/0/0"
                }
            });
        }

        cardProfile.UploadReplays.Add(new UploadReplay()
        {
            Filename = preSaveRequest.MobileUserId + "_" + preSaveRequest.PlayedAt,
            ReplaySize = preSaveRequest.ReplaySize,
            PlayedAt = preSaveRequest.PlayedAt,
            StageId = preSaveRequest.StageId,
            PilotsJson = JsonConvert.SerializeObject(preSaveRequest.Pilots),
            SpecialFlag = preSaveRequest.SpecialFlag,
            ReplayServiceFlag = preSaveRequest.ReplayServiceFlag,
            MobileUserId = preSaveRequest.MobileUserId,
            MatchingMode = preSaveRequest.MatchingMode,
            TeamType = preSaveRequest.TeamType,
            ReturnMatchFlag = preSaveRequest.ReturnMatchFlag,
            TournamentId = preSaveRequest.TournamentId
        });

        _context.SaveChanges();

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            pre_save_replay = new Response.PreSaveReplay()
            {
                Status = 1,
                Url = $"http://{request.BaseAddress}/upload/uploadReplay/{preSaveRequest.MobileUserId}/{preSaveRequest.PlayedAt}"
            }
        });
    }
}