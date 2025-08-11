using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.ViewCard;
using WebUIOver.Shared.Dto.History;

namespace WebUIOver.Client.Command.ViewCard.Filler;

public class BattleHistoriesFiller : IViewCardContextFiller
{
    private HttpClient _httpClient;
    
    public BattleHistoriesFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(ViewCardContext viewCardContext)
    {
        var battleHistories = await _httpClient.GetFromJsonAsync<List<BattleHistorySummary>>($"/ui/battle-history/get-recent-battle-histories/{viewCardContext.AccessCode}/{viewCardContext.ChipId}");
        battleHistories.ThrowIfNull();

        viewCardContext.BattleHistorySummaries = battleHistories;
    }
}