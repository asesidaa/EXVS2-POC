using nue.protocol.exvs;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.MobileSuit;

namespace ServerOver.Processor.Tracker;

public interface IStickerTrackerProcessor
{
    void Process(Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker tracker, MobileSuitPvPStatistic? mobileSuitPvPStatistic, PlayerLevel playerLevel);
}