using System;
using System.Linq;

namespace WebUI.Shared.Dto.Request;

public class CardAuthorizationRequest
{
    public string AccessCode { get; init; } = string.Empty;

    public string ChipId { get; init; } = string.Empty;
}
