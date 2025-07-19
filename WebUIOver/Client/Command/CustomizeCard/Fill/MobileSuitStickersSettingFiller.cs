using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class MobileSuitStickersSettingFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    
    public MobileSuitStickersSettingFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var msStickerDtos = await _httpClient.GetFromJsonAsync<List<StickerDto>>($"/ui/sticker/getMobileSuitStickers/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        msStickerDtos.ThrowIfNull();

        customizeCardContext.MobileSuitStickerSettings = msStickerDtos;
    }
}