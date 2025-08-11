using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

public interface IPreLoadPlayerCommand
{
    void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer);
}