using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerOver.Mapper.Card.Triad;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Triad;

public record GetCpuTriadPartnerCommand(string AccessCode, string ChipId) : IRequest<CpuTriadPartner>;

public class GetCpuTriadPartnerCommandHandler : IRequestHandler<GetCpuTriadPartnerCommand, CpuTriadPartner>
{
    private readonly ServerDbContext _context;
    
    public GetCpuTriadPartnerCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<CpuTriadPartner> Handle(GetCpuTriadPartnerCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.TriadPartner)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        return Task.FromResult(cardProfile.TriadPartner.ToCpuTriadPartner());
    }
}