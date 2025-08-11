using WebUIOver.Shared.Dto.Enum;

namespace WebUIOver.Shared.Dto.Training;

public class TrainingProfile
{
    public uint MstMobileSuitId { get; set; } = 0;
    public BurstType BurstType { get; set; } = 0;
    public uint CpuLevel { get; set; } = 0;
    public uint ExBurstGauge { get; set; } = 0;
    public bool DamageDisplay { get; set; } = true;
    public bool CpuAutoGuard { get; set; } = true;
    public bool CommandGuideDisplay { get; set; } = true;
}