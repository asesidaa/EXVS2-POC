using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Sticker;

public interface IStickerService
{
    public Task InitializeAsync();
    public TrackerType? GetTrackerTypeById(uint id);
    public IReadOnlyList<TrackerType> GetTrackerTypesSortedById();
    public GeneralPreview? GetStickerBackgroundById(uint id);
    public IReadOnlyList<GeneralPreview> GetStickerBackgroundsSortedById();
    public GeneralPreview? GetStickerEffectById(uint id);
    public IReadOnlyList<GeneralPreview> GetStickerEffectsSortedById();
}