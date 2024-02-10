using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class ExMatchingTableFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.ExMatchingTables.Add(new Response.LoadGameData.ExtremeMatchMatchingTable()
        {
            MatchingTryNum = 1,
            SeqNum = 2001,
            MakeRoomFlag = false
        });
        
        loadGameData.ExMatchingTables.Add(new Response.LoadGameData.ExtremeMatchMatchingTable()
        {
            MatchingTryNum = 2,
            SeqNum = 2002,
            MakeRoomFlag = false
        });
        
        loadGameData.ExMatchingTables.Add(new Response.LoadGameData.ExtremeMatchMatchingTable()
        {
            MatchingTryNum = 3,
            SeqNum = 2003,
            MakeRoomFlag = false
        });
        
        loadGameData.ExMatchingTables.Add(new Response.LoadGameData.ExtremeMatchMatchingTable()
        {
            MatchingTryNum = 4,
            SeqNum = 2004,
            MakeRoomFlag = false
        });
        
        loadGameData.ExMatchingTables.Add(new Response.LoadGameData.ExtremeMatchMatchingTable()
        {
            MatchingTryNum = 5,
            SeqNum = 2005,
            MakeRoomFlag = true
        });
    }
}