using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveLicenseScoreCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    
    public SaveLicenseScoreCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var onlineDomain = battleResultContext.OnlineBattleDomain;
        var scoreChange = onlineDomain.LicenseScoreChange;
        
        var licenseScoreRecord = _context.LicenseScoreRecordDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        if (licenseScoreRecord is null)
        {
            cardProfile.LicenseScoreRecord = new LicenseScoreRecord()
            {
                LicenseScore = 0,
                LastObtainedScore = 0,
                CardProfile = cardProfile
            };

            licenseScoreRecord = cardProfile.LicenseScoreRecord;
        }
        
        int finalScore = (int) licenseScoreRecord.LicenseScore + scoreChange;

        if (finalScore <= 0)
        {
            licenseScoreRecord.LicenseScore = 0;
            return;
        }

        licenseScoreRecord.LicenseScore = (uint) finalScore;
        licenseScoreRecord.LastObtainedScore = scoreChange;
    }
}