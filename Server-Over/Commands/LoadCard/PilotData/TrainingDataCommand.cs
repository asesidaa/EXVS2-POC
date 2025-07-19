using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class TrainingDataCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public TrainingDataCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var trainingProfile = _context.TrainingProfileDbSet
            .First(x => x.CardProfile == cardProfile);
        
        pilotDataGroup.Training = new Response.LoadCard.PilotDataGroup.TrainingSettingGroup()
        {
            MstMobileSuitId = trainingProfile.MstMobileSuitId,
            BurstType = trainingProfile.BurstType,
            CpuLevel = trainingProfile.CpuLevel,
            ExBurstGauge = trainingProfile.ExBurstGauge,
            DamageDisplay = trainingProfile.DamageDisplay,
            CpuAutoGuard = trainingProfile.CpuAutoGuard,
            CommandGuideDisplay = trainingProfile.CommandGuideDisplay,
        };
    }
}