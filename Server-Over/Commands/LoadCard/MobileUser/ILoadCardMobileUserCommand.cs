using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.LoadCard.MobileUser;

public interface ILoadCardMobileUserCommand
{
    void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup);
}