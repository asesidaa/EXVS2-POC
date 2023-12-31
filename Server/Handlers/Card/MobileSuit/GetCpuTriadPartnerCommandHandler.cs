﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Mappers;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

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
        var updateRequest = request;
        
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        var mobileUserGroup = JsonConvert.DeserializeObject<Response.LoadCard.MobileUserGroup>(cardProfile.UserDomain.MobileUserGroupJson);

        if (user == null || mobileUserGroup == null)
        {
            throw new InvalidCardDataException("Card Content is invalid");
        }

        var cpuTriadPartner = mobileUserGroup.ToCpuTriadPartner();
        cpuTriadPartner.Skill1 = user.customize_group.MsSkill1;
        cpuTriadPartner.Skill2 = user.customize_group.MsSkill2;
        
        return Task.FromResult(cpuTriadPartner);
    }
}