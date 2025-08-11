using WebUIOver.Client.Services.Common;
using WebUIOver.Client.Services.MS;
using WebUIOver.Client.Services.Navi;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Name
{
    public class NameService : INameService
    {
        private readonly INaviDataService _naviDataService;
        private readonly IMobileSuitDataService _mobileSuitDataService;

        public NameService(INaviDataService naviDataService, IMobileSuitDataService mobileSuitDataService)
        {
            _naviDataService = naviDataService;
            _mobileSuitDataService = mobileSuitDataService;
        }
        
        public string GetNavigatorName(uint id)
        {
            var navigator = _naviDataService.GetNavigatorById(id);
            var localizedName = GetLocalizedName(navigator);
            return localizedName ?? "Unknown Navigator";
        }
        
        public string GetNavigatorSeriesName(uint id)
        {
            var navigator = _naviDataService.GetNavigatorById(id);
            var localizedName = GetLocalizedNaviSeriesName(navigator);
            return localizedName ?? "Unknown Series";
        }
        
        public string GetNavigatorSeiyuuName(uint id)
        {
            var navigator = _naviDataService.GetNavigatorById(id);
            var localizedName = GetLocalizedNaviSeiyuuName(navigator);
            return localizedName ?? "Unknown Seiyuu";
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
                case "zh-Hant":
                    returnString = obj.SeriesTC;
                    break;
                case "zh-Hant-TW":
                    returnString = obj.SeriesTC2;
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
                case "zh-Hant":
                    returnString = obj.SeiyuuTC;
                    break;
                case "zh-Hant-TW":
                    returnString = obj.SeiyuuTC2;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Seiyuu;

            return returnString;
        }
        
        public string GetMobileSuitName(uint id)
        {
            var mobilesuit = _mobileSuitDataService.GetMobileSuitById(id);
            var localizedName = GetLocalizedName(mobilesuit);
            return localizedName ?? "Unknown Mobile Suit";
        }
        
        public string GetMobileSuitPilotName(uint id)
        {
            var mobilesuit = _mobileSuitDataService.GetMobileSuitById(id);
            var localizedName = GetLocalizedPilotName(mobilesuit);
            return localizedName ?? "Unknown Pilot";
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
                case "zh-Hant":
                    returnString = obj.PilotTC;
                    break;
                case "zh-Hant-TW":
                    returnString = obj.PilotTC2;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Pilot;

            return returnString;
        }
        
        public string GetMobileSuitSeriesName(uint id)
        {
            var mobilesuit = _mobileSuitDataService.GetMobileSuitById(id);
            var localizedName = GetLocalizedMobileSuitSeriesName(mobilesuit);
            return localizedName ?? "Unknown Series";
        }
        
        public string? GetLocalizedMobileSuitSeriesName(MobileSuit? obj)
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
                case "zh-Hant":
                    returnString = obj.SeriesTC;
                    break;
                case "zh-Hant-TW":
                    returnString = obj.SeriesTC2;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Series;

            return returnString;
        }
        
        public string GetMobileSuitSeiyuuName(uint id)
        {
            var mobilesuit = _mobileSuitDataService.GetMobileSuitById(id);
            var localizedName = GetLocalizedMobileSuitSeiyuuName(mobilesuit);
            return localizedName ?? "Unknown Series";
        }
        
        public string? GetLocalizedMobileSuitSeiyuuName(MobileSuit? obj)
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
                    returnString = obj.PilotSeiyuu;
                    break;
                case "ja":
                    returnString = obj.PilotSeiyuuJP;
                    break;
                case "zh-Hans":
                    returnString = obj.PilotSeiyuuCN;
                    break;
                case "zh-Hant":
                    returnString = obj.PilotSeiyuuTC;
                    break;
                case "zh-Hant-TW":
                    returnString = obj.PilotSeiyuuTC2;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.PilotSeiyuu;

            return returnString;
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
                case "zh-Hant":
                    returnString = obj.ValueTC;
                    break;
                case "zh-Hant-TW":
                    returnString = obj.ValueTC2;
                    break;
            }

            // assume value is always filled
            if (string.IsNullOrWhiteSpace(returnString))
                returnString = obj.Value;

            return returnString;
        }
    }
}
