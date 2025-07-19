using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public interface IPreLoadMobileUserGroupCommand
{
    void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup);
}