namespace WebUI.Shared.Dto.Request;

public class PreCreateTeamRequest : BasicCardRequest
{
    public uint PartnerCardId { get; set; } = 0;
}