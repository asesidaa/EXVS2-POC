using nue.protocol.exvs;

namespace Server.Handlers.Game.LoadGameData;

public class OfflineWinLossEchelonCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.OfflineWinEchelonNums.Add(new Response.LoadGameData.OfflineEchelon()
        {
            Id = 1,
            UpperThreshold = 9,
            LowerThreshold = 0,
            Point = 0
        });
        
        loadGameData.OfflineWinEchelonNums.Add(new Response.LoadGameData.OfflineEchelon()
        {
            Id = 2,
            UpperThreshold = 15,
            LowerThreshold = 10,
            Point = 5
        });
        
        loadGameData.OfflineWinEchelonNums.Add(new Response.LoadGameData.OfflineEchelon()
        {
            Id = 3,
            UpperThreshold = 55,
            LowerThreshold = 16,
            Point = 10
        });
        
        loadGameData.OfflineLoseEchelonNums.Add(new Response.LoadGameData.OfflineEchelon()
        {
            Id = 1,
            UpperThreshold = 9,
            LowerThreshold = 0,
            Point = 0
        });
        
        loadGameData.OfflineLoseEchelonNums.Add(new Response.LoadGameData.OfflineEchelon()
        {
            Id = 2,
            UpperThreshold = 15,
            LowerThreshold = 10,
            Point = 5
        });
        
        loadGameData.OfflineLoseEchelonNums.Add(new Response.LoadGameData.OfflineEchelon()
        {
            Id = 3,
            UpperThreshold = 55,
            LowerThreshold = 16,
            Point = 10
        });
    }
}