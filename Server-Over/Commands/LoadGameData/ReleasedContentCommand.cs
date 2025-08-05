using nue.protocol.exvs;
using ServerOver.Models.Config;

namespace ServerOver.Commands.LoadGameData;

public class ReleasedContentCommand : ILoadGameDataCommand
{
    private readonly CardServerConfig _config;

    public ReleasedContentCommand(CardServerConfig config)
    {
        _config = config;
    }
    
    public void Fill(Response.LoadGameData loadGameData)
    {
        var releasedMobileSuits = Enumerable.Range(1, 400).Select(i => (uint)i).ToArray();
        var releasedNavis = Enumerable.Range(1, 51).Select(i => (uint)i).ToArray();
        var newMobileSuits = _config.GameConfigurations.NewMobileSuits ?? new[] { 135u, 323u, 326u };
        var updatedMobileSuits = _config.GameConfigurations.UpdatedMobileSuits ?? [
            3u, 7u, 13u, 37u, 54u, 58u,
            62u, 80u, 92u, 105u, 107u, 108u,
            113u, 114u, 116u, 121u, 131u, 144u,
            151u, 157u, 169u, 180u, 181u, 183u,
            191u, 192u, 194u, 195u, 199u, 209u,
            222u, 287u, 288u, 291u, 305u, 306u,
            310u, 313u, 321u, 322u
        ];

        loadGameData.ReleaseMsIds = releasedMobileSuits;
        loadGameData.DisplayableMsIds = releasedMobileSuits;
        loadGameData.ReleaseGuestNavIds = releasedNavis;
        loadGameData.NewMsIds = newMobileSuits;
        loadGameData.UpdateMsIds = updatedMobileSuits;
        loadGameData.MobileSuitBlocklists = new[] { 5u, 39u, 58u, 198u, 214u, 215u, 218u, 241u, 256u, 288u };
    }
}