using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PostProcess;

public class LatestPlayTimeUpdateCommand : ILoadCardPostProcessCommand
{
    private readonly ServerDbContext _context;
    
    public LatestPlayTimeUpdateCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void PostProcess(CardProfile cardProfile, Request.LoadCard loadCardRequest)
    {
        cardProfile.LastPlayedAt = (ulong) DateTimeOffset.Now.ToUnixTimeSeconds();
        _context.SaveChanges();
    }
}