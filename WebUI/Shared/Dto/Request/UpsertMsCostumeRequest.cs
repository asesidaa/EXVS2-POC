using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpsertMsCostumeRequest : BasicCardRequest
{
    public BaseMobileSuit MobileSuit { get; set; } = new();
}