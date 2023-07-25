using Microsoft.AspNetCore.Mvc;

namespace Server.Models;

public class MuchaUpdateCheckRequest
{
    [FromForm(Name = "gameCd")]
    public string? GameCode { get; set; }
    
    [FromForm(Name = "gameVer")]
    public string? GameVersion { get; set; }
    
    [FromForm(Name = "serialNum")]
    public string? SerialNumber { get; set; }
    
    [FromForm(Name = "countryCd")]
    public string? CountryCode { get; set; }
    
    [FromForm(Name = "placeId")]
    public string? PlaceId { get; set; }
    
    [FromForm(Name = "storeRouterIp")]
    public string? StoreRouterIp { get; set; }
}