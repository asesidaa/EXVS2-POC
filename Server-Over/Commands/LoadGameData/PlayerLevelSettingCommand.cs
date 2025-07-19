using nue.protocol.exvs;
using ServerOver.Constants;

namespace ServerOver.Commands.LoadGameData;

public class PlayerLevelSettingCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.PlayerLevelExp2 = PlayerLevelExp.Round2Exp;
        loadGameData.PlayerLevelExp3 = (int) PlayerLevelExp.Round3Exp;
        loadGameData.PlayerLevelExp4 = PlayerLevelExp.RoundExExp;
        loadGameData.PLevelBonusRate = 1.2f;
    }
}