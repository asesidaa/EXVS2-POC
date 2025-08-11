using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

public class WinLossDataCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;

    public WinLossDataCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var winLossRecord = _context.WinLossRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        loadPlayer.TotalWin = winLossRecord.TotalWin;
        loadPlayer.TotalLose = winLossRecord.TotalLose;
        loadPlayer.ShuffleWin = winLossRecord.ShuffleWin;
        loadPlayer.ShuffleLose = winLossRecord.ShuffleLose;
        loadPlayer.TeamWin = winLossRecord.TeamWin;
        loadPlayer.TeamLose = winLossRecord.TeamLose;
        loadPlayer.ClassSoloWin = winLossRecord.ClassSoloWin;
        loadPlayer.ClassSoloLose = winLossRecord.ClassSoloLose;
        loadPlayer.ClassTeamWin = winLossRecord.ClassTeamWin;
        loadPlayer.ClassTeamLose = winLossRecord.ClassTeamLose;
        loadPlayer.FesWin = winLossRecord.FesWin;
        loadPlayer.FesLose = winLossRecord.FesLose;
        loadPlayer.FreeWin = winLossRecord.FreeWin;
        loadPlayer.FreeLose = winLossRecord.FreeLose;
    }
}