using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class TeamResponseFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    
    public TeamResponseFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var teamResponse = await _httpClient.GetFromJsonAsync<TeamResponse>($"/ui/team/getTeams/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        teamResponse.ThrowIfNull();

        customizeCardContext.TeamResponse = teamResponse;
    }
}