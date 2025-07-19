using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpdateAllNaviCostumeRequest : BasicCardRequest
{
    public List<Navi> Navis { get; set; } = new();
}