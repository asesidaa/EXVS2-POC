using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Training;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class TrainingProfileFiller : ICustomizeCardContextFiller
{
    private HttpClient _httpClient;
    
    public TrainingProfileFiller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var trainingProfile = await _httpClient.GetFromJsonAsync<TrainingProfile>($"/ui/training/get/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        trainingProfile.ThrowIfNull();

        customizeCardContext.TrainingProfile = trainingProfile;
    }
}