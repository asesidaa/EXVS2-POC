namespace WebUIOver.Client.Validator;

public interface INameValidator
{
    public string? ValidatePlayerName(string playerName);
    public string? ValidateTeamName(string teamName);
    public string? ValidatePvPTeamName(string teamName);
    public string? ValidateCustomizeMessage(string message);
    public IEnumerable<string> ValidateMessageEnumerable(string message);
    public string? ValidateCustomizeTitle(string message);
}