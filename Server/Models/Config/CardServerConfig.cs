using System.Text.Json;

namespace Server.Models.Config;

public class CardServerConfig
{
    public const string CARD_SERVER_SECTION = "CardServerConfig";

    public bool IgnoreUploadSaving { get; set; }
    public bool NeedToDataPatchOfflineData { get; set; }

    public GameConfigurations GameConfigurations { get; set; } = new();

    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        var jsonString = JsonSerializer.Serialize(this, options);
        return jsonString;
    }
}