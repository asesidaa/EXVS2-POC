namespace WebUIVanilla.Shared.Dto.Request;

public class CardAuthorizationRequest
{
    public int CardId { get; init; } = 0;
    public string AccessCode { get; init; } = string.Empty;

    public string ChipId { get; init; } = string.Empty;
}
