using Microsoft.AspNetCore.Mvc;

namespace Server.Models;

public class MuchaBoardAuthRequest
{
    [FromForm(Name = "gameVer")]
    public string? GameVersion { get; set; }

    [FromForm(Name = "sendDate")]
    public string? SendDate { get; set; }

    [FromForm(Name = "serialNum")]
    public string? SerialNumber { get; set; }

    [FromForm(Name = "gameCd")]
    public string? GameCode { get; set; }

    [FromForm(Name = "boardType")]
    public string? BoardType { get; set; }

    [FromForm(Name = "boardId")]
    public string? BoardId { get; set; }
    
    [FromForm(Name = "placeId")]
    public string? PlaceId { get; set; }

    [FromForm(Name = "storeRouterIp")]
    public string? StoreRouterIp { get; set; }

    [FromForm(Name = "countryCd")]
    public string? CountryCode { get; set; }

    [FromForm(Name = "useToken")]
    public string? UseToken { get; set; }

    [FromForm(Name = "allToken")]
    public string? AllToken { get; set; }

}