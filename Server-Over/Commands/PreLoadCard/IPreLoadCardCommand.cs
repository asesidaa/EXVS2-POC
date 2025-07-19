using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.PreLoadCard;

public interface IPreLoadCardCommand
{
    void Fill(CardProfile cardProfile, Response.PreLoadCard preLoadCard);
}