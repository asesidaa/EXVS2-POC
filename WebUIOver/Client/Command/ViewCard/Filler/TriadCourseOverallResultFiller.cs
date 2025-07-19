using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.ViewCard;
using WebUIOver.Shared.Dto.Triad;

namespace WebUIOver.Client.Command.ViewCard.Filler;

public class TriadCourseOverallResultFiller : IViewCardContextFiller
{
    private HttpClient _httpClient;
    
    public TriadCourseOverallResultFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(ViewCardContext viewCardContext)
    {
        var triadCourseOverallResult = await _httpClient.GetFromJsonAsync<TriadCourseOverallResult>($"/ui/triad/getTriadCourseOverallResult/{viewCardContext.AccessCode}/{viewCardContext.ChipId}");
        triadCourseOverallResult.ThrowIfNull();

        viewCardContext.TriadCourseOverallResult = triadCourseOverallResult;
    }
}