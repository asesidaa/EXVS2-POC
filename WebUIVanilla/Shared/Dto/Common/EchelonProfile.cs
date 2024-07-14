namespace WebUIVanilla.Shared.Dto.Common;

public class EchelonProfile
{
    public uint EchelonId { get; set; } = 0;
    public bool SpecialEchelonFlag { get; set; } = false;
    public bool SCaptainFlag { get; set; } = false;
    public bool SBrigadierFlag { get; set; } = false;
    public int EchelonExp { get; set; } = 0;
    public bool AppliedForSpecialEchelonTest { get; set; } = false; 
    public uint SpecialEchelonTestProgress { get; set; } = 0;
}