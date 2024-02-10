using nue.protocol.exvs;
using ServerVanilla.Mapper.Card.Common;
using ServerVanilla.Persistence;

namespace ServerVanilla.Command.LoadGameData;

public class EchelonTableFiller : ILoadGameDataFiller
{
    private readonly ServerDbContext _context;
    
    public EchelonTableFiller(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Fill(Response.LoadGameData loadGameData)
    {
        _context.EchelonSettings
            .OrderBy(x => x.EchelonId)
            .ToList()
            .ForEach(echelonSetting => loadGameData.EchelonTables.Add(echelonSetting.ToEchelonTable()));
        
        loadGameData.TeamEchelonCoefficient = 0;
    }
    
}