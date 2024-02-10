using ServerVanilla.Context.Battle;
using ServerVanilla.Models.Cards;

namespace ServerVanilla.Command.SaveBattle;

public interface ISaveBattleDataCommand
{
    void Save(CardProfile cardProfile, BattleResultContext battleResultContext);
}