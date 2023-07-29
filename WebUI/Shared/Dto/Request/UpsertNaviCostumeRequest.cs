namespace WebUI.Shared.Dto.Request;

public class UpsertNaviCostumeRequest : BasicCardRequest
{
    public uint naviId { get; set; } = 1;
    public uint costumeId { get; set; } = 0;
}