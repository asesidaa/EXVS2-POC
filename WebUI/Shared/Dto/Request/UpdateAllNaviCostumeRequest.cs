using WebUI.Shared.Dto.Common;

namespace WebUI.Shared.Dto.Request;

public class UpdateAllNaviCostumeRequest : BasicCardRequest
{
    public List<Navi> Navis { get; set; } = new();
}