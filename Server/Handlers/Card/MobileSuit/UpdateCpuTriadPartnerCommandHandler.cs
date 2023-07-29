using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record UpdateCpuTriadPartnerCommand(UpdateCpuTriadPartnerRequest Request) : IRequest<BasicResponse>;

public class UpdateCpuTriadPartnerCommandHandler : IRequestHandler<UpdateCpuTriadPartnerCommand, BasicResponse>
{
    private readonly ServerDbContext context;
    
    public UpdateCpuTriadPartnerCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<BasicResponse> Handle(UpdateCpuTriadPartnerCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;
        
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
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

        user.customize_group.MstMobileSuitId = updateRequest.CpuTriadPartner.MobileSuitId;
        user.customize_group.MsSkill1 = updateRequest.CpuTriadPartner.Skill1;
        user.customize_group.MsSkill2 = updateRequest.CpuTriadPartner.Skill2;

        mobileUserGroup.MstMobileSuitId = updateRequest.CpuTriadPartner.MobileSuitId;
        mobileUserGroup.BurstType = (uint)updateRequest.CpuTriadPartner.BurstType;
        mobileUserGroup.ArmorLevel = updateRequest.CpuTriadPartner.ArmorLevel;
        mobileUserGroup.ShootAttackLevel = updateRequest.CpuTriadPartner.ShootAttackLevel;
        mobileUserGroup.InfightAttackLevel = updateRequest.CpuTriadPartner.InfightAttackLevel;
        mobileUserGroup.BoosterLevel = updateRequest.CpuTriadPartner.BoosterLevel;
        mobileUserGroup.ExGaugeLevel = updateRequest.CpuTriadPartner.ExGaugeLevel;
        mobileUserGroup.AiLevel = updateRequest.CpuTriadPartner.AiLevel;
        mobileUserGroup.TriadTeamName = updateRequest.CpuTriadPartner.TriadTeamName;
        mobileUserGroup.TriadBackgroundPartsId = updateRequest.CpuTriadPartner.TriadBackgroundPartsId;

        cardProfile.UserDomain.UserJson = JsonConvert.SerializeObject(user);
        cardProfile.UserDomain.MobileUserGroupJson = JsonConvert.SerializeObject(mobileUserGroup);
        
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}