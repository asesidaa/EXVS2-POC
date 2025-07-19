namespace ServerOver.Dtos.Response.AmActivator;

public class SignatureResponse
{
    public int Status { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
    public int Signature { get; set; } = 0;
}