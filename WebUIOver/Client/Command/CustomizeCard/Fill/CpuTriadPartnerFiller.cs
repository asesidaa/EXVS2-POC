using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class CpuTriadPartnerFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;

    public CpuTriadPartnerFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var cpuTriadPartner = await _httpClient.GetFromJsonAsync<CpuTriadPartner>($"/ui/triad/getCpuTriadPartner/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        cpuTriadPartner.ThrowIfNull();

        customizeCardContext.CpuTriadPartner = cpuTriadPartner;
    }
}