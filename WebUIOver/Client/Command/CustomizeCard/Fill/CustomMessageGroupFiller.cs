using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Message;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class CustomMessageGroupFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    
    public CustomMessageGroupFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var customMessageGroupSetting = await _httpClient.GetFromJsonAsync<CustomMessageGroupSetting>($"/ui/message/getCustomMessageGroupSetting/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        customMessageGroupSetting.ThrowIfNull();

        customizeCardContext.CustomMessageGroupSetting = customMessageGroupSetting;
    }
}