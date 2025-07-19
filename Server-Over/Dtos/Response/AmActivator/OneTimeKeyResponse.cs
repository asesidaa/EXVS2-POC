namespace ServerOver.Dtos.Response.AmActivator;

public class OneTimeKeyResponse
{
    public int Status { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
    public int Otk { get; set; } = 0;
    public string Uuid { get; set; } = string.Empty;
    public string ExpiredAt { get; set; } = string.Empty;
}