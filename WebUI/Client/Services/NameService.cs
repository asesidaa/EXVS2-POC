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

        public string GetNavigatorName(uint id)
        {
            var navigator = _service.GetNavigatorById(id);
            var localizedName = GetLocalizedName(navigator);
            return localizedName ?? "Unknown Navigator";
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
    }
}
