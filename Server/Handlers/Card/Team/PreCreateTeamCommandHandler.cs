﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace Server.Handlers.Card.Team;

public record PreCreateTeamCommand(PreCreateTeamRequest Request) : IRequest<PreCreateTeamResponse>;

public class PreCreateTeamCommandHandler : IRequestHandler<PreCreateTeamCommand, PreCreateTeamResponse>
{
    private readonly ServerDbContext _context;
    
    public PreCreateTeamCommandHandler(ServerDbContext context)
    {
        _context = context;
    }
    
    public Task<PreCreateTeamResponse> Handle(PreCreateTeamCommand request, CancellationToken cancellationToken)
    {
        var preCreateTeamRequestRequest = request.Request;
        
        var cardProfile = _context.CardProfiles
            .Include(x => x.PilotDomain)
            .Include(x => x.TagTeamDataList)
            .FirstOrDefault(x => x.AccessCode == preCreateTeamRequestRequest.AccessCode && x.ChipId == preCreateTeamRequestRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        if (cardProfile.TagTeamDataList.Count >= 20)
        {
            return Task.FromResult(new PreCreateTeamResponse
            {
                Success = true,
                NewTeamId = 0
            });
        }

        var cardId = cardProfile.Id;

        var existingTeam = _context.TagTeamData
            .ToList()
            .FirstOrDefault(team =>
            {
                if (team.CardId == cardId && team.TeammateCardId == preCreateTeamRequestRequest.PartnerCardId)
                {
                    return true;
                }
                
                if (team.CardId == preCreateTeamRequestRequest.PartnerCardId && team.TeammateCardId == cardId)
                {
                    return true;
                }

                return false;
            });

        if (existingTeam is not null)
        {
            return Task.FromResult(new PreCreateTeamResponse
            {
                Success = true,
                NewTeamId = 0
            });
        }

        var newTeam = new TagTeamData
        {
            CardId = cardProfile.Id,
            TeammateCardId = preCreateTeamRequestRequest.PartnerCardId,
            TeamName = "EXTREME TEAM",
            BackgroundPartsId = 0,
            EffectId = 0,
            EmblemId = 0,
            SkillPoint = 0,
            SkillPointBoost = 0,
            BgmId = 0,
            NameColorId = 0
        };

        cardProfile.TagTeamDataList.Add(newTeam);

        _context.SaveChanges();
        
        return Task.FromResult(new PreCreateTeamResponse
        {
            Success = true,
            NewTeamId = (uint) newTeam.Id,
        });
    }
}