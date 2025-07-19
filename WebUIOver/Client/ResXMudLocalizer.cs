using Microsoft.Extensions.Localization;
using MudBlazor;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client
{
    internal class ResXMudLocalizer : MudLocalizer
    {
        private IStringLocalizer _localization;

        public ResXMudLocalizer(IStringLocalizer<Resource> localizer)

        {
            _localization = localizer;
        }

        public override LocalizedString this[string key] => _localization[key];
    }
}
