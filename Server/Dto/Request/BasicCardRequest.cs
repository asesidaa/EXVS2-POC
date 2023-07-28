namespace Server.Dto.Request;

public class BasicCardRequest
{
    public String accessCode { get; set; } = String.Empty;
    public String chipId { get; set; } = String.Empty;
}