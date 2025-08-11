using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Name
{
    public interface INameService
    {
        public string GetNavigatorName(uint id);
        public string GetNavigatorSeriesName(uint id);
        public string GetNavigatorSeiyuuName(uint id);
        public string GetMobileSuitName(uint id);
        public string GetMobileSuitPilotName(uint id);
        public string GetMobileSuitSeriesName(uint id);
        public string GetMobileSuitSeiyuuName(uint id);
        public string? GetLocalizedName(IdValuePair? obj);
    }
}
