using System.Text.Json;

namespace ServerOver.Models.Config;

public class CardServerConfig
{
    public const string CARD_SERVER_SECTION = "CardServerConfig";
    
    public bool DisableTelop { get; set; } = false;
    public uint MaxReplaySaveSlotPerPlayer { get; set; }

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