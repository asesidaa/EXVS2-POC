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
        var newMobileSuits = _config.GameConfigurations.NewMobileSuits ?? new[] { 322u };
        var updatedMobileSuits = _config.GameConfigurations.UpdatedMobileSuits ?? [
            7u, 92u, 96u, 113u, 126u, 165u, 176u,
            211u, 222u, 228u, 231u, 284u
        ];

        loadGameData.ReleaseMsIds = releasedMobileSuits;
        loadGameData.DisplayableMsIds = releasedMobileSuits;
        loadGameData.ReleaseGuestNavIds = releasedNavis;
        loadGameData.NewMsIds = newMobileSuits;
        loadGameData.UpdateMsIds = updatedMobileSuits;
        loadGameData.MobileSuitBlocklists = new[] { 5u, 39u, 58u, 198u, 214u, 215u, 218u, 241u, 256u, 288u };
    }
}