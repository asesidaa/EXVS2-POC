using nue.protocol.exvs;

namespace Server.Handlers.Game.LoadGameData;

public class CasualMatchCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.CasualMatchingTables.Add(new Response.LoadGameData.CasualMatchingTable()
        {
            MatchingRank = 0,
            MatchingTryNum = 1,
            SeqNum = 3,
            MakeRoomFlag = true
        });
        
        loadGameData.CasualMatchingTables.Add(new Response.LoadGameData.CasualMatchingTable()
        {
            MatchingRank = 1,
            MatchingTryNum = 1,
            SeqNum = 4,
            MakeRoomFlag = true
        });
        
        loadGameData.CasualMatchingTables.Add(new Response.LoadGameData.CasualMatchingTable()
        {
            MatchingRank = 2,
            MatchingTryNum = 1,
            SeqNum = 2,
            MakeRoomFlag = true
        });
        
        loadGameData.CasualMatchRankTables.Add(new Response.LoadGameData.CasualMatchingRankTable()
        {
            WinNum = 0,
            MatchingRank = 0
        });
        
        loadGameData.CasualMatchRankTables.Add(new Response.LoadGameData.CasualMatchingRankTable()
        {
            WinNum = 1,
            MatchingRank = 0
        });

        for (uint winNum = 2; winNum < 5; winNum++)
        {
            loadGameData.CasualMatchRankTables.Add(new Response.LoadGameData.CasualMatchingRankTable()
            {
                WinNum = winNum,
                MatchingRank = 1
            });
        }
        
        for (uint winNum = 5; winNum < 10; winNum++)
        {
            loadGameData.CasualMatchRankTables.Add(new Response.LoadGameData.CasualMatchingRankTable()
            {
                WinNum = winNum,
                MatchingRank = 2
            });
        }
    }
}