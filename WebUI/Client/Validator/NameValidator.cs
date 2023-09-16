using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using WebUI.Client.Constants;
using WebUI.Shared.Resources;

namespace WebUI.Client.Validator;

public class NameValidator : INameValidator
{
    private IStringLocalizer<Resource> _localizer;
    
    public NameValidator(IStringLocalizer<Resource> localizer)
    {
        _localizer = localizer;
    }
    
    public string? ValidatePlayerName(string playerName)
    {
        return ValidateName(playerName, _localizer["validateplayername"]);
    }
    
    public string? ValidateTeamName(string teamName)
    {
        return ValidateName(teamName, _localizer["validatetriadteamname"]);
    }
    
    public string? ValidateCustomizeMessage(string message)
    {
        return ValidateMessage(message, _localizer["validatemessage"]);
    }
    
    private String? ValidateName(string name, string errorMessagePart)
    {
        const string pattern = @"^[ 一-龯ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９-_ー＜＞＋－＊÷＝；：←／＼＿｜・＠！？＆★（）＾◇∀Ξν×†ω♪♭#∞〆→↓↑％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]「」『』【】]{1,12}$";

        return name.Length switch
        {
            0 => errorMessagePart + $" {_localizer["validation_required"]}",
            > (int) NameLength.PlayerNameMaxLength => errorMessagePart + $" {_localizer["validate_length_1"]} 12 {_localizer["validate_length_2"]}",
            _ => !Regex.IsMatch(name, pattern) ? errorMessagePart + $" {_localizer["validation_invalidchar"]}" : null
        };
    }
    
    private String? ValidateMessage(string message, string errorMessagePart)
    {
        const string pattern = @"^[ 一-龯ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９ー＜＞＋－＊÷＝；：←／＼＿｜・＠！？＆★（）＾◇∀Ξν×†ω♪♭#∞〆→↓↑％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]「」『』【】]{1,10}$";

        return message.Length switch
        {
            0 => null,
            > (int) NameLength.MessageMaxLength => errorMessagePart + $" {_localizer["validate_length_1"]} 10 {_localizer["validate_length_2"]}",
            _ => !Regex.IsMatch(message, pattern) ? errorMessagePart + $" {_localizer["validation_invalidchar"]}" : null
        };
    }
}