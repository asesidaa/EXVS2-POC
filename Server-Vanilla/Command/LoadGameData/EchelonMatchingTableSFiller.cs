using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class EchelonMatchingTableSFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 0,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = true
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 1,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 1,
            MatchingTryNum = 2,
            SeqNum = 3,
            MakeRoomFlag = true
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 2,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 2,
            MatchingTryNum = 2,
            SeqNum = 3,
            MakeRoomFlag = true
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 3,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 3,
            MatchingTryNum = 2,
            SeqNum = 2,
            MakeRoomFlag = true
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 4,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 4,
            MatchingTryNum = 2,
            SeqNum = 3,
            MakeRoomFlag = true
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 5,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 5,
            MatchingTryNum = 2,
            SeqNum = 2,
            MakeRoomFlag = true
        });

        for (uint matchRank = 6; matchRank < 9; matchRank++)
        {
            loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = matchRank,
                MatchingTryNum = 1,
                SeqNum = 1,
                MakeRoomFlag = true
            });
        }
        
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 9,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 9,
            MatchingTryNum = 2,
            SeqNum = 3,
            MakeRoomFlag = true
        });

        for (uint matchRank = 10; matchRank < 13; matchRank++)
        {
            loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = matchRank,
                MatchingTryNum = 1,
                SeqNum = 1,
                MakeRoomFlag = false
            });
            loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
            {
                OnlineMatchRank = matchRank,
                MatchingTryNum = 2,
                SeqNum = 4,
                MakeRoomFlag = true
            });
        }
        
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 13,
            MatchingTryNum = 1,
            SeqNum = 1,
            MakeRoomFlag = false
        });
        loadGameData.EchelonMatchingTablesSs.Add(new Response.LoadGameData.EchelonMatchingTable()
        {
            OnlineMatchRank = 13,
            MatchingTryNum = 2,
            SeqNum = 2,
            MakeRoomFlag = true
        });
    }
}