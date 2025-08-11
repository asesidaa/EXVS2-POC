using System.Net.Http.Json;
using Throw;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.CustomizeComment;

public class CustomizeCommentService : ICustomizeCommentService
{
    private readonly HttpClient _client;
    private readonly ILogger<CustomizeCommentService> _logger;
    
    private Dictionary<uint, IdValuePair> _customizeCommentSentences = new();
    private Dictionary<uint, IdValuePair> _customizeCommentPhrases = new();
    private List<IdValuePair> _customizeCommentSentenceList = new();
    private List<IdValuePair> _customizeCommentPhraseList = new();

    public CustomizeCommentService(HttpClient client, ILogger<CustomizeCommentService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var customizeCommentSentencesList = await _client.GetFromJsonAsync<List<IdValuePair>>("data/comment/CustomizeComment.json");
        customizeCommentSentencesList.ThrowIfNull();
        _customizeCommentSentences = customizeCommentSentencesList.ToDictionary(ms => ms.Id);
        _customizeCommentSentenceList = customizeCommentSentencesList.OrderBy(title => title.Id).ToList();
        
        var customizeCommentPhrasesList = await _client.GetFromJsonAsync<List<IdValuePair>>("data/comment/CustomizeCommentPhrase.json");
        customizeCommentPhrasesList.ThrowIfNull();
        _customizeCommentPhrases = customizeCommentPhrasesList.ToDictionary(ms => ms.Id);
        _customizeCommentPhraseList = customizeCommentPhrasesList.OrderBy(title => title.Id).ToList();
    }

    public IdValuePair? GetCustomizeCommentSentenceById(uint id)
    {
        return _customizeCommentSentences.GetValueOrDefault(id);
    }

    public IReadOnlyList<IdValuePair> GetCustomizeCommentSentenceSortedById()
    {
        return _customizeCommentSentenceList;
    }
    
    public IdValuePair? GetCustomizeCommentPhraseById(uint id)
    {
        return _customizeCommentPhrases.GetValueOrDefault(id);
    }

    public IReadOnlyList<IdValuePair> GetCustomizeCommentPhraseSortedById()
    {
        return _customizeCommentPhraseList;
    }
    
    public string GetCustomizeCommentSentenceName(uint id)
    {
        var customizeCommentSentence = GetCustomizeCommentSentenceById(id);
        if (customizeCommentSentence is null)
        {
            return "Unknown Sentence";
        }
            
        return customizeCommentSentence.Value;
    }
        
    public string GetCustomizeCommentPhraseName(uint id)
    {
        var customizeCommentSentence = GetCustomizeCommentPhraseById(id);
        if (customizeCommentSentence is null)
        {
            return "Unknown Phrase";
        }
            
        return customizeCommentSentence.Value;
    }
}