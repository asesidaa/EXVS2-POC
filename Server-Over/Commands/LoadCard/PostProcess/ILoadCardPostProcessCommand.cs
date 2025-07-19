using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.LoadCard.PostProcess;

public interface ILoadCardPostProcessCommand
{
    void PostProcess(CardProfile cardProfile, Request.LoadCard loadCardRequest);
}