namespace WebUI.Client.Validator;

public interface INameValidator
{
    public string? ValidatePlayerName(string playerName);
    public string? ValidateTeamName(string teamName);
    public string? ValidateCustomizeMessage(string message);
}