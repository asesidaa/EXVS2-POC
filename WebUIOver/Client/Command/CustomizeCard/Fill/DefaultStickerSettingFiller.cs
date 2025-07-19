using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class DefaultStickerSettingFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    
    public DefaultStickerSettingFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var defaultSticker = await _httpClient.GetFromJsonAsync<StickerDto>($"/ui/sticker/getDefaultSticker/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        defaultSticker.ThrowIfNull();

        customizeCardContext.DefaultStickerSetting = defaultSticker;
    }
}