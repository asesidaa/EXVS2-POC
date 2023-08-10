namespace WebUI.Client.Helpers
{
    public static class GetLocalizedFriendlyName
    {
        public static string? GetLocalizedName(object? obj, ILogger log = null)
        {
            if (obj == null)
                return null;

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;

            // use reflection to get NameEN, NameJP or NameCN
            // very ugly code since i am too lazy to change all implementation of NameEN, NameJP and NameCN to IdValuePairs
            var objProperties = obj.GetType().GetProperties();
            var relevantProperties = objProperties.Where(x => x.Name == "NameEN" || x.Name == "NameJP" || x.Name == "NameCN");
            var idValuePairProperties = objProperties.Where(x => x.Name == "Value" || x.Name == "ValueJP" || x.Name == "ValueCN");

            var returnString = string.Empty;

            if (relevantProperties.Any())
            {
                switch (cultureName)
                {
                    case "en-US":
                        returnString = relevantProperties.FirstOrDefault(x => x.Name == "NameEN")?.GetValue(obj) as string;
                        break;
                    case "ja":
                        returnString = relevantProperties.FirstOrDefault(x => x.Name == "NameJP")?.GetValue(obj) as string;
                        break;
                    case "zh-Hans":
                        returnString = relevantProperties.FirstOrDefault(x => x.Name == "NameCN")?.GetValue(obj) as string;
                        break;
                }

                if (string.IsNullOrWhiteSpace(returnString))
                    returnString = relevantProperties.FirstOrDefault(x => x.Name == "NameEN")?.GetValue(obj) as string;
            }
            else if (idValuePairProperties.Any())
            {
                switch (cultureName)
                {
                    case "en-US":
                        returnString = idValuePairProperties.FirstOrDefault(x => x.Name == "Value")?.GetValue(obj) as string;
                        break;
                    case "ja":
                        returnString = idValuePairProperties.FirstOrDefault(x => x.Name == "ValueJP")?.GetValue(obj) as string;
                        break;
                    case "zh-Hans":
                        returnString = idValuePairProperties.FirstOrDefault(x => x.Name == "ValueCN")?.GetValue(obj) as string;
                        break;
                }

                if (string.IsNullOrWhiteSpace(returnString))
                    returnString = idValuePairProperties.FirstOrDefault(x => x.Name == "Value")?.GetValue(obj) as string;
            }

            return returnString;
        }
    }
}
