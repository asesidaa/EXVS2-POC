using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Request;
using WebUIVanilla.Shared.Dto.Response;

namespace ServerVanilla.Handlers.Card.MobileSuit;

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
            .Include(x => x.TriadPartner)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        cardProfile.TriadPartner.MstMobileSuitId = updateRequest.CpuTriadPartner.MobileSuitId;
        cardProfile.TriadPartner.MsSkill1 = updateRequest.CpuTriadPartner.Skill1;
        cardProfile.TriadPartner.MsSkill2 = updateRequest.CpuTriadPartner.Skill2;
        cardProfile.TriadPartner.BurstType = (uint)updateRequest.CpuTriadPartner.BurstType;
        cardProfile.TriadPartner.ArmorLevel = (uint)updateRequest.CpuTriadPartner.ArmorLevel;
        cardProfile.TriadPartner.ShootAttackLevel = (uint)updateRequest.CpuTriadPartner.ShootAttackLevel;
        cardProfile.TriadPartner.InfightAttackLevel = (uint)updateRequest.CpuTriadPartner.InfightAttackLevel;
        cardProfile.TriadPartner.BoosterLevel = (uint)updateRequest.CpuTriadPartner.BoosterLevel;
        cardProfile.TriadPartner.ExGaugeLevel = (uint)updateRequest.CpuTriadPartner.ExGaugeLevel;
        cardProfile.TriadPartner.AiLevel = (uint)updateRequest.CpuTriadPartner.AiLevel;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}