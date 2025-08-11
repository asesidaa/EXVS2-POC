using ServerOver.Context.Battle;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.SaveBattle;

public interface ISaveBattleDataCommand
{
    void Save(CardProfile cardProfile, BattleResultContext battleResultContext);
}