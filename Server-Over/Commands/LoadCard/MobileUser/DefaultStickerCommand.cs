using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Processor.Tracker;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class DefaultStickerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private readonly IStickerTrackerProcessor _stickerTrackerProcessor;

    public DefaultStickerCommand(ServerDbContext context, IStickerTrackerProcessor stickerTrackerProcessor)
    {
        _context = context;
        _stickerTrackerProcessor = stickerTrackerProcessor;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var defaultStickerProfile = _context.DefaultStickerProfileDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var defaultSticker = new Response.LoadCard.MobileUserGroup.PlayerSticker()
        {
            MstMobileSuitId = 0,
            PoseId = 0,
            BasePanelId = defaultStickerProfile.BasePanelId,
            CommentPartsAId = defaultStickerProfile.CommentPartsAId,
            CommentPartsBId = defaultStickerProfile.CommentPartsBId,
            StickerBackgroundId = defaultStickerProfile.StickerBackgroundId,
            StickerEffectId = defaultStickerProfile.StickerEffectId,
        };

        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);
        
        // Disable this part is want to screenshot raw Sticker, excluding Tracker
        var defaultTrackers = new List<uint>
            {
                defaultStickerProfile.Tracker1,
                defaultStickerProfile.Tracker2,
                defaultStickerProfile.Tracker3
            }
            .Where(tracker => tracker > 0)
            .ToList();
        
        defaultTrackers.ForEach(defaultTracker =>
        {
            var tracker = new Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker()
            {
                TextId = defaultTracker
            };
            
            _stickerTrackerProcessor.Process(tracker, null, playerLevel);
            
            defaultSticker.Trackers.Add(tracker);
        });
        
        mobileUserGroup.PlayerStickers.Add(defaultSticker);
    }
}