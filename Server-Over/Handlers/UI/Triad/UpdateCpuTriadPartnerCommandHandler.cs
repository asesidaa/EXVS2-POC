using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Handlers.UI.Triad;

public record UpdateCpuTriadPartnerCommand(UpdateCpuTriadPartnerRequest Request) : IRequest<BasicResponse>;

public class UpdateCpuTriadPartnerCommandHandler : IRequestHandler<UpdateCpuTriadPartnerCommand, BasicResponse>
{
    private readonly ServerDbContext _context;
    
    public UpdateCpuTriadPartnerCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateCpuTriadPartnerCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        var triadPartner = _context.TriadPartnerDbSet
            .First(x => x.CardProfile == cardProfile);
        
        triadPartner.MstMobileSuitId = updateRequest.CpuTriadPartner.MobileSuitId;
        triadPartner.MsSkill1 = updateRequest.CpuTriadPartner.Skill1;
        triadPartner.MsSkill2 = updateRequest.CpuTriadPartner.Skill2;
        triadPartner.BurstType = (uint)updateRequest.CpuTriadPartner.BurstType;
        triadPartner.ArmorLevel = (uint)updateRequest.CpuTriadPartner.ArmorLevel;
        triadPartner.ShootAttackLevel = (uint)updateRequest.CpuTriadPartner.ShootAttackLevel;
        triadPartner.InfightAttackLevel = (uint)updateRequest.CpuTriadPartner.InfightAttackLevel;
        triadPartner.BoosterLevel = (uint)updateRequest.CpuTriadPartner.BoosterLevel;
        triadPartner.ExGaugeLevel = (uint)updateRequest.CpuTriadPartner.ExGaugeLevel;
        triadPartner.AiLevel = (uint)updateRequest.CpuTriadPartner.AiLevel;
        triadPartner.TriadTeamName = updateRequest.CpuTriadPartner.TriadTeamName;
        triadPartner.TriadBackgroundPartsId = updateRequest.CpuTriadPartner.TriadBackgroundPartsId;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}