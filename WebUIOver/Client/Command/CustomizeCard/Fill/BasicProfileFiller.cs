using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class BasicProfileFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    
    public BasicProfileFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var profileResult = await _httpClient.GetFromJsonAsync<BasicProfile>($"/ui/card/getBasicDisplayProfile/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        profileResult.ThrowIfNull();

        customizeCardContext.BasicProfile = profileResult;
    }
}