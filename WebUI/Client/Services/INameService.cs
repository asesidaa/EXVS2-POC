using WebUI.Shared.Dto.Common;

namespace WebUI.Client.Services
{
    public interface INameService
    {
        public string GetMobileSuitName(uint id);
        public string GetMobileSuitPilotName(uint id);
        public string GetNavigatorName(uint id);
        public string GetNavigatorSeriesName(uint id);
        public string GetNavigatorSeiyuuName(uint id);
        public string GetGaugeName(uint id);
        public string? GetLocalizedName(IdValuePair? obj);
        public string GetCustomizeCommentSentenceName(uint id);
        public string GetCustomizeCommentPhraseName(uint id);
    }
}
