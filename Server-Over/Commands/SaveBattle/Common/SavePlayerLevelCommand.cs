using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Common;

public class SavePlayerLevelCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;
    private const uint NormalMaxLv = 200;
    private const uint ExMaxLv = 999;

    public SavePlayerLevelCommand(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var playerLevelDomain = battleResultContext.PlayerLevelDomain;
        var playerLevelData = _context.PlayerLevelDbSet.First(x => x.CardProfile == cardProfile);
        var maxLevel = GetMaxLevel(playerLevelData);

        // Skip Processing if LevelIdBefore >= 200 / 999, because EXP can't be further incremented
        if (playerLevelDomain.LevelIdBefore >= maxLevel)
        {
            return;
        }

        if (playerLevelDomain.LevelIdBefore == playerLevelDomain.LevelIdAfter)
        {
            playerLevelData.PlayerExp += playerLevelDomain.ExpIncrement;
            return;
        }

        playerLevelData.PlayerLevelId = playerLevelDomain.LevelIdAfter;
        playerLevelData.PlayerExp = 0;

        // If Level After >= 200 and PrestigeId < 3, eligible to increment Prestige ID and reset Player Lv to 1 through Web UI
        if (playerLevelDomain.LevelIdAfter >= 200 && playerLevelDomain.PrestigeId < 3)
        {
            playerLevelData.LevelMaxDispFlag = true;
        }
    }

    private uint GetMaxLevel(PlayerLevel playerLevelData)
    {
        if (playerLevelData.PrestigeId < 3)
        {
            return NormalMaxLv;
        }

        return ExMaxLv;
    }
}