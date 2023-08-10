using Riok.Mapperly.Abstractions;
using Server.Models.Cards;
using WebUI.Shared.Dto.Response;

namespace Server.Mappers;

[Mapper]
public static partial class UploadedImageMapper
{
    [MapProperty(nameof(UploadImage.CreateTime), nameof(UploadedImage.CreateTime))]
    public static partial UploadedImage ToUploadedImage(this UploadImage uploadImage);
}