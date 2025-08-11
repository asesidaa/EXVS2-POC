using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Persistence;
using ServerOver.Processor.Tracker;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class MobileSuitStickerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private readonly IStickerTrackerProcessor _pilotTrackerProcessor;
    private readonly IStickerTrackerProcessor _mobileSuitStickerTrackerProcessor;

    public MobileSuitStickerCommand(ServerDbContext context, IStickerTrackerProcessor pilotTrackerProcessor, IStickerTrackerProcessor mobileSuitStickerTrackerProcessor)
    {
        _context = context;
        _pilotTrackerProcessor = pilotTrackerProcessor;
        _mobileSuitStickerTrackerProcessor = mobileSuitStickerTrackerProcessor;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var mobileSuitStickers = _context.MobileSuitStickerDbSet
            .Where(x => x.CardProfile == cardProfile)
            .OrderBy(x => x.MstMobileSuitId)
            .ToList();
        
        mobileSuitStickers.ForEach(mobileSuitSticker =>
        {
            if (HasBlankSticker(mobileSuitSticker))
            {
                return;
            }
            
            var msPlayerSticker = new Response.LoadCard.MobileUserGroup.PlayerSticker()
            {
                MstMobileSuitId = mobileSuitSticker.MstMobileSuitId,
                PoseId = mobileSuitSticker.PoseId,
                BasePanelId = 0,
                CommentPartsAId = 0,
                CommentPartsBId = 0,
                StickerBackgroundId = mobileSuitSticker.StickerBackgroundId,
                StickerEffectId = mobileSuitSticker.StickerEffectId,
            };

            var msTrackers = new List<uint>
                {
                    mobileSuitSticker.Tracker1,
                    mobileSuitSticker.Tracker2,
                    mobileSuitSticker.Tracker3
                }
                .Where(tracker => tracker > 0)
                .ToList();
            
            msTrackers.ForEach(msTracker =>
            {
                var tracker = new Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker()
                {
                    TextId = msTracker
                };

                ProcessMobileSuitTracker(tracker, cardProfile, mobileSuitSticker, playerLevel);
                
                msPlayerSticker.Trackers.Add(tracker);
            });
            
            mobileUserGroup.PlayerStickers.Add(msPlayerSticker);
        });
    }

    bool HasBlankSticker(MobileSuitSticker mobileSuitSticker)
    {
        var isBlankTracker = mobileSuitSticker.Tracker1 == 0 && mobileSuitSticker.Tracker2 == 0 &&
                             mobileSuitSticker.Tracker3 == 0;
        var isBlankDisplay = mobileSuitSticker.StickerBackgroundId == 0 && mobileSuitSticker.StickerEffectId == 0;
        var isWithoutPose = mobileSuitSticker.PoseId == 0;

        return isBlankTracker && isBlankDisplay && isWithoutPose;
    }

    void ProcessMobileSuitTracker(Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker tracker, 
        CardProfile cardProfile, MobileSuitSticker mobileSuitSticker, PlayerLevel playerLevel)
    {
        var mobilePvPStat = _context.MobileSuitPvPStatisticDbSet
            .FirstOrDefault(x =>
                x.CardProfile == cardProfile &&
                x.MstMobileSuitId == mobileSuitSticker.MstMobileSuitId
            );

        if (mobilePvPStat is null)
        {
            _pilotTrackerProcessor.Process(tracker, null, playerLevel);
            return;
        }
        
        _mobileSuitStickerTrackerProcessor.Process(tracker, mobilePvPStat, playerLevel);
    }
}