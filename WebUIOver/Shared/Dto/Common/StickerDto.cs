namespace WebUIOver.Shared.Dto.Common;

public class StickerDto
{
    public uint MobileSuitId { get; set; } = 0;
    
    public uint StickerBackgroundId { get; set; } = 0;
    public uint StickerEffectId { get; set; } = 0;
    public uint Tracker1 { get; set; } = 0;
    public uint Tracker2 { get; set; } = 0;
    public uint Tracker3 { get; set; } = 0;
    
    // MS Exclusive
    public uint PoseId { get; set; } = 0;
    
    // Default Exclusive
    public uint BasePanelId { get; set; } = 0;
    public uint CommentPartsAId { get; set; } = 0;
    public uint CommentPartsBId { get; set; } = 0;
}