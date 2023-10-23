namespace WebUI.Shared.Dto.Common;

public class Team
{
    public uint Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public uint PartnerId { get; set; } = 0;
    public string PartnerName { get; set; } = string.Empty;
    public uint BackgroundPartsId { get; set; } = 0;
    public uint EffectId { get; set; } = 0;
    public uint EmblemId { get; set; } = 0;
    public uint SkillPoint { get; set; } = 0;
    public uint SkillPointBoost { get; set; } = 0;
    public uint BgmId { get; set; } = 0;
    public uint NameColorId { get; set; } = 0;
}