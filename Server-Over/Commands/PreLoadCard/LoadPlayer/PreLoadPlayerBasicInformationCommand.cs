using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

public class PreLoadPlayerBasicInformationCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;

    public PreLoadPlayerBasicInformationCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);
        
        loadPlayer.PilotId = (uint)cardProfile.Id;
        loadPlayer.LastPlayedAt = cardProfile.LastPlayedAt;
        loadPlayer.VsmAfterRankUp = 0;
        loadPlayer.ExTutorialPlayableFlag = playerProfile.ExTutorialDispFlag;
    }
}