namespace WebUIVanilla.Shared.Dto.Request;

public class UpdateEchelonTestSettingRequest : BasicCardRequest
{
    public bool ParticipateInTest { get; set; } = false;
}