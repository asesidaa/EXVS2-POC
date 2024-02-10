using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerVanilla.Mapper.Card.Triad;
using ServerVanilla.Persistence;
using WebUIVanilla.Shared.Dto.Common;

namespace ServerVanilla.Handlers.Card.MobileSuit;

public record GetCpuTriadPartnerCommand(string AccessCode, string ChipId) : IRequest<CpuTriadPartner>;

public class GetCpuTriadPartnerCommandHandler : IRequestHandler<GetCpuTriadPartnerCommand, CpuTriadPartner>
{
    private readonly ServerDbContext context;
    
    public GetCpuTriadPartnerCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<CpuTriadPartner> Handle(GetCpuTriadPartnerCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.TriadPartner)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        return Task.FromResult(cardProfile.TriadPartner.ToCpuTriadPartner());
    }
}