using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveVstResultCommand(Request Request) : IRequest<Response>;

public class SaveVstResultCommandHandler : IRequestHandler<SaveVstResultCommand, Response>
{
    private readonly ILogger<SaveVstResultCommandHandler> _logger;
    private readonly ServerDbContext _context;

    public SaveVstResultCommandHandler(ILogger<SaveVstResultCommandHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Task<Response> Handle(SaveVstResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vst_result.SessionId;
        var cardId = request.Request.save_vst_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return Task.FromResult(CreateStandardResponse(request));
        }

        var vstResult = request.Request.save_vst_result;

        if (vstResult.TutorialFlag)
        {
            return Task.FromResult(CreateStandardResponse(request));
        }

        if (vstResult.TrainingType is not 0)
        {
            return Task.FromResult(CreateStandardResponse(request));
        }

        var trainingSetting = _context.TrainingProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        trainingSetting.CpuLevel = vstResult.CpuLevel;
        trainingSetting.ExBurstGauge = vstResult.ExBurstGauge;
        trainingSetting.DamageDisplay = vstResult.DamageDisplay;
        trainingSetting.CpuAutoGuard = vstResult.CpuAutoGuard;
        trainingSetting.CommandGuideDisplay = vstResult.CommandGuideDisplay;
        trainingSetting.MstMobileSuitId = vstResult.LastMobileSuitId;
        trainingSetting.BurstType = vstResult.LastBurstType;

        _context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vst_result = new Response.SaveVstResult()
        });
    }

    Response CreateStandardResponse(SaveVstResultCommand request)
    {
        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vst_result = new Response.SaveVstResult()
        };
    }
}
