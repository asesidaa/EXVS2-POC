namespace WebUIOver.Shared.Dto.Response;

public class PlayerExistenceResult : BasicResponse
{
    public uint PlayerId { get; set; } = 0;
    public string PlayerName { get; set; } = string.Empty;
}