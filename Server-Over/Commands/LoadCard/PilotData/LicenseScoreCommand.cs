using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class LicenseScoreCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public LicenseScoreCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        var licenseScoreRecord = _context.LicenseScoreRecordDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        if (licenseScoreRecord is null)
        {
            pilotDataGroup.LicenseTrackerScore = 0;
            pilotDataGroup.LastObtainedScore = 0;

            cardProfile.LicenseScoreRecord = new LicenseScoreRecord()
            {
                CardProfile = cardProfile,
                LicenseScore = 0,
                LastObtainedScore = 0
            };
            
            _context.SaveChanges();
            
            return;
        }

        pilotDataGroup.LicenseTrackerScore = licenseScoreRecord.LicenseScore;
        pilotDataGroup.LastObtainedScore = licenseScoreRecord.LastObtainedScore;
    }
}