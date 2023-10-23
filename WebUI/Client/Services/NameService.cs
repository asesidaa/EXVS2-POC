using WebUI.Shared.Dto.Common;

namespace WebUI.Client.Services
{
    public class NameService : INameService
    {
        private IDataService _service;

        public NameService(IDataService service)
        {
            _service = service;
        }

        public string GetMobileSuitName(uint id)
        {
            var mobilesuit = _service.GetMobileSuitById(id);
            var localizedName = GetLocalizedName(mobilesuit);
            return localizedName ?? "Unknown Mobile Suit";
        }
        
        public string GetMobileSuitPilotName(uint id)
        {
            var mobilesuit = _service.GetMobileSuitById(id);
            var localizedName = GetLocalizedPilotName(mobilesuit);
            return localizedName ?? "Unknown Pilot";
        }
        
        public string GetNavigatorName(uint id)
        {
            var navigator = _service.GetNavigatorById(id);
            var localizedName = GetLocalizedName(navigator);
            return localizedName ?? "Unknown Navigator";
        }
        
        public string GetNavigatorSeriesName(uint id)
        {
            var navigator = _service.GetNavigatorById(id);
            var localizedName = GetLocalizedNaviSeriesName(navigator);
            return localizedName ?? "Unknown Series";
        }
        
        public string GetNavigatorSeiyuuName(uint id)
        {
            var navigator = _service.GetNavigatorById(id);
            var localizedName = GetLocalizedNaviSeiyuuName(navigator);
            return localizedName ?? "Unknown Seiyuu";
        }

        public string GetGaugeName(uint id)
        {
            var gauge = _service.GetGaugeById(id);
            var localizedName = GetLocalizedName(gauge);
            return localizedName ?? "Unknown Gauge";
        }
        
        public string? GetLocalizedName(IdValuePair? obj)
        {
            if (obj == null)
                return null;

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;

            // use reflection to get the different properties type.
            var returnString = string.Empty;

            switch (cultureName)
            {
                default:
                case "en-US":
                    returnString = obj.Value;
                    break;
                case "ja":
                    returnString = obj.ValueJP;
                    break;
                case "zh-Hans":
                    returnString = obj.ValueCN;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Value;

            return returnString;
        }
        
        public string? GetLocalizedPilotName(MobileSuit? obj)
        {
            if (obj == null)
                return null;

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;

            // use reflection to get the different properties type.
            var returnString = string.Empty;

            switch (cultureName)
            {
                default:
                case "en-US":
                    returnString = obj.Pilot;
                    break;
                case "ja":
                    returnString = obj.PilotJP;
                    break;
                case "zh-Hans":
                    returnString = obj.PilotCN;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Pilot;

            return returnString;
        }
        
        public string? GetLocalizedNaviSeriesName(Navigator? obj)
        {
            if (obj == null)
                return null;

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;

            // use reflection to get the different properties type.
            var returnString = string.Empty;

            switch (cultureName)
            {
                default:
                case "en-US":
                    returnString = obj.Series;
                    break;
                case "ja":
                    returnString = obj.SeriesJP;
                    break;
                case "zh-Hans":
                    returnString = obj.SeriesCN;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Series;

            return returnString;
        }
        
        public string? GetLocalizedNaviSeiyuuName(Navigator? obj)
        {
            if (obj == null)
                return null;

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;

            // use reflection to get the different properties type.
            var returnString = string.Empty;

            switch (cultureName)
            {
                default:
                case "en-US":
                    returnString = obj.Seiyuu;
                    break;
                case "ja":
                    returnString = obj.SeiyuuJP;
                    break;
                case "zh-Hans":
                    returnString = obj.SeiyuuCN;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Seiyuu;

            return returnString;
        }
        
        public string GetCustomizeCommentSentenceName(uint id)
        {
            var customizeCommentSentence = _service.GetCustomizeCommentSentenceById(id);
            if (customizeCommentSentence is null)
            {
                return "Unknown Sentence";
            }
            
            return customizeCommentSentence.Value;
        }
        
        public string GetCustomizeCommentPhraseName(uint id)
        {
            var customizeCommentSentence = _service.GetCustomizeCommentPhraseById(id);
            if (customizeCommentSentence is null)
            {
                return "Unknown Phrase";
            }
            
            return customizeCommentSentence.Value;
        }
    }
}
