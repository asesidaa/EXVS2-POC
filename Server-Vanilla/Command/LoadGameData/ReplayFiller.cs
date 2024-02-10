using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class ReplayFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.ReplayUnderEchelonId = 0; // In reality it is 9
        loadGameData.AdvancedReplayUnderEchelonId = 0; // In reality it is 19
    }
}