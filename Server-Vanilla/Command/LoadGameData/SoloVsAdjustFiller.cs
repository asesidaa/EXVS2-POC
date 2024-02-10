using nue.protocol.exvs;

namespace ServerVanilla.Command.LoadGameData;

public class SoloVsAdjustFiller : ILoadGameDataFiller
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 1,
            Max = 60,
            Min = 60,
            Num = 5
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 2,
            Max = 59,
            Min = 55,
            Num = 3
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 3,
            Max = 54,
            Min = 31,
            Num = 2
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 4,
            Max = 30,
            Min = 21,
            Num = 1
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 5,
            Max = 20,
            Min = -3,
            Num = 0
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 6,
            Max = -4,
            Min = -10,
            Num = -1
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 7,
            Max = -11,
            Min = -20,
            Num = -3
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 8,
            Max = -21,
            Min = -59,
            Num = -5
        });
        
        loadGameData.VsAdjustSs.Add(new Response.LoadGameData.EchelonMatchingVsAdjustTable()
        {
            RankId = 9,
            Max = -60,
            Min = -60,
            Num = -10
        });
    }
}