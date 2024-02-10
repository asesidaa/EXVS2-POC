using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class MobileSuitFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        var allMsIds = Enumerable.Range(1, 294).Select(i => (uint)i).ToArray();

        loadGameData.ReleaseMsIds = allMsIds;
        loadGameData.NewMsIds = Array.Empty<uint>();
        loadGameData.DisplayableMsIds = allMsIds;
    }
}