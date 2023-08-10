namespace WebUI.Shared.Dto.Response;

public class UploadedImage
{
    public int ImageId { get; set; }
    public string Filename { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
}