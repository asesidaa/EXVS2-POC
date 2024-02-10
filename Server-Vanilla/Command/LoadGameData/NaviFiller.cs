using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class NaviFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        var allNaviIds = Enumerable.Range(0, 43).Select(i => (uint)i).ToArray();
        
        loadGameData.ReleaseGuestNavIds = allNaviIds;
    }
}