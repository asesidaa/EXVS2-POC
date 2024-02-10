using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class EchelonMatchingTableTFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        for (uint onlineMatchRank = 0; onlineMatchRank < 4; onlineMatchRank++)
        {
            loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = onlineMatchRank,
                MatchingTryNum = 1,
                SeqNum = 1,
                MakeRoomFlag = false
            });
            loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = onlineMatchRank,
                MatchingTryNum = 2,
                SeqNum = 3,
                MakeRoomFlag = true
            });
        }
        
        for (uint onlineMatchRank = 4; onlineMatchRank < 6; onlineMatchRank++)
        {
            loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = onlineMatchRank,
                MatchingTryNum = 1,
                SeqNum = 1,
                MakeRoomFlag = false
            });
            loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = onlineMatchRank,
                MatchingTryNum = 2,
                SeqNum = 4,
                MakeRoomFlag = true
            });
        }
        
        loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 6,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 6,
            MatchingTryNum = 2,
            SeqNum = 3,
            MakeRoomFlag = true
        });
        
        loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 7,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesTs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 7,
            MatchingTryNum = 2,
            SeqNum = 4,
            MakeRoomFlag = true
        });
    }
}