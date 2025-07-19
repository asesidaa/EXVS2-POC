using nue.protocol.exvs;

namespace ServerOver.Commands.LoadGameData;

public class FreeMatchDataCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.FreeMatchingTables.Add(new Response.LoadGameData.FreeMatchingTable()
        {
            MatchingRank = 0,
            MatchingTryNum = 1,
            SeqNum = 3,
            MakeRoomFlag = true
        });
        
        loadGameData.FreeMatchingTables.Add(new Response.LoadGameData.FreeMatchingTable()
        {
            MatchingRank = 1,
            MatchingTryNum = 1,
            SeqNum = 4,
            MakeRoomFlag = true
        });
        
        loadGameData.FreeMatchingTables.Add(new Response.LoadGameData.FreeMatchingTable()
        {
            MatchingRank = 2,
            MatchingTryNum = 1,
            SeqNum = 2,
            MakeRoomFlag = true
        });

        for (uint i = 0; i < 2; i++)
        {
            loadGameData.FreeMatchRankTables.Add(new Response.LoadGameData.FreeMatchingRankTable()
            {
                MatchingRank = i,
                WinNum = 0
            });
        }
        
        for (uint i = 2; i < 5; i++)
        {
            loadGameData.FreeMatchRankTables.Add(new Response.LoadGameData.FreeMatchingRankTable()
            {
                MatchingRank = i,
                WinNum = 1
            });
        }
        
        for (uint i = 5; i < 10; i++)
        {
            loadGameData.FreeMatchRankTables.Add(new Response.LoadGameData.FreeMatchingRankTable()
            {
                MatchingRank = i,
                WinNum = 2
            });
        }
    }
}