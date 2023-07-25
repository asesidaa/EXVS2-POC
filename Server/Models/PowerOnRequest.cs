using Microsoft.AspNetCore.Mvc;

namespace Server.Models;

public class PowerOnRequest
{
    [FromForm(Name = "game_id")]
    public string? GameId { get; set; }
    
    [FromForm(Name = "ver")]
    public string? Version { get; set; }
    
    [FromForm(Name = "serial")]
    public string? Serial { get; set; }
    
    [FromForm(Name = "ip")]
    public string? Ip { get; set; }
    
    [FromForm(Name = "firm_ver")]
    public string? FirmwareVersion { get; set; }
    
    [FromForm(Name = "boot_ver")]
    public string? BootVersion { get; set; }
    
    [FromForm(Name = "encode")]
    public string? Encode { get; set; }
    
    [FromForm(Name = "format_ver")]
    public string? FormatVersion { get; set; }
    
    [FromForm(Name = "hops")]
    public string? Hops { get; set; }
}