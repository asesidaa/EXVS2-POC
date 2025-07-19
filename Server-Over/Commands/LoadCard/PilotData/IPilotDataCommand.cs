using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.LoadCard.PilotData;

public interface IPilotDataCommand
{
    void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup);
}