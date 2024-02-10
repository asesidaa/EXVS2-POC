namespace WebUIVanilla.Shared.Dto.Request;

public class UpsertNaviCostumeRequest : BasicCardRequest
{
    public uint NaviId { get; set; } = 1;
    public uint CostumeId { get; set; } = 0;
}