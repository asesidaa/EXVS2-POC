namespace ServerOver.Dtos.Request;

public class TokenRequest
{
    public string Token { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
}