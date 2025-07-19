using nue.protocol.exvs;
using ServerOver.Models.Config;

namespace ServerOver.Commands.LoadGameData;

public class TriadWantedDataCommand : ILoadGameDataCommand
{
    private readonly CardServerConfig _config;

    public TriadWantedDataCommand(CardServerConfig config)
    {
        _config = config;
    }
    
    public void Fill(Response.LoadGameData loadGameData)
    {
        loadGameData.WantedAttackLevel = 2;
        loadGameData.WantedPsAttackLevel = 2;
        loadGameData.WantedPsDefenceLevel = 1;
        loadGameData.WantedDownLevel = 2000; // this will cause enemy to 1 hit down if set to 1

        if (_config.GameConfigurations.TriadConfigurations.TargetMsList.Length == 0)
        {
            return;
        }

        loadGameData.MstMobileSuitIds = _config.GameConfigurations.TriadConfigurations.TargetMsList;
    }
}