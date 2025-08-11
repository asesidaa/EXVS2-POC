using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Preview;

public class GeneralPreviewService : IGeneralPreviewService
{
    public Dictionary<uint, GeneralPreview> CreateGeneralPreviewDictionary(List<GeneralPreview> generalPreviews)
    {
        return generalPreviews
            .Where(generalPreview => generalPreview.Existence != "NotExist")
            .ToDictionary(generalPreview => generalPreview.Id);
    }

    public List<GeneralPreview> CreateSortedGeneralPreviewList(List<GeneralPreview> generalPreviews)
    {
        return generalPreviews
            .Where(generalPreview => generalPreview.Existence != "NotExist")  
            .OrderBy(generalPreview => generalPreview.Id)
            .ToList();
    }
}