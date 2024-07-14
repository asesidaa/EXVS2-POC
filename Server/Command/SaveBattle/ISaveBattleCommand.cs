using nue.protocol.exvs;
using Server.Context.Battle;
using Server.Models.Cards;

namespace Server.Command.SaveBattle;

public interface ISaveBattleCommand
{
    void Save(CardProfile cardProfile, 
        Response.PreLoadCard.LoadPlayer loadPlayer, 
        Response.PreLoadCard.MobileUserGroup user,
        Response.LoadCard.PilotDataGroup pilotData, 
        Response.LoadCard.MobileUserGroup mobileUser, 
        BattleResultContext battleResultContext
    );
}