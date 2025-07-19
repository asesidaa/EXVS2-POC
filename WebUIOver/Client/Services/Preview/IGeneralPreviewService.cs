using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Preview;

public interface IGeneralPreviewService
{
    Dictionary<uint, GeneralPreview> CreateGeneralPreviewDictionary(List<GeneralPreview> generalPreviews);
    List<GeneralPreview> CreateSortedGeneralPreviewList(List<GeneralPreview> generalPreviews);
}