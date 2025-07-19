namespace ServerOver.Dtos.Request.AmActivator;

public class SignatureRequest
{
    public string BoardSerial { get; set; } = string.Empty;
    public int Otk { get; set; } = 0;
    public string Uuid { get; set; } = string.Empty;
}