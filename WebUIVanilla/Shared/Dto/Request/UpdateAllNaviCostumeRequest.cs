using WebUIVanilla.Shared.Dto.Common;

namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateAllNaviCostumeRequest : BasicCardRequest
{
    public List<Navi> Navis { get; set; } = new();
}