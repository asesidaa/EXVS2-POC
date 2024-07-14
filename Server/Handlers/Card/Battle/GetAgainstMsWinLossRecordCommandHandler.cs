using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Battle;

public record GetAgainstMsWinLossRecordCommand(string AccessCode, string ChipId, string Mode) : IRequest<List<MsBattleRecord>>;

public class GetAgainstMsWinLossRecordCommandHandler : IRequestHandler<GetAgainstMsWinLossRecordCommand, List<MsBattleRecord>>
{
    private readonly ServerDbContext context;
    private readonly ILogger<GetAgainstMsWinLossRecordCommand> _logger;
    
    public GetAgainstMsWinLossRecordCommandHandler(ServerDbContext context, ILogger<GetAgainstMsWinLossRecordCommand> logger)
    {
        this.context = context;
        _logger = logger;
    }

    public Task<List<MsBattleRecord>> Handle(GetAgainstMsWinLossRecordCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.OfflinePvpBattleResults)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }

        if (cardProfile.OfflinePvpBattleResults.Count == 0)
        {
            return Task.FromResult(new List<MsBattleRecord>());
        }

        var battleRecords = new List<MsBattleRecord>(400);
        
        cardProfile.OfflinePvpBattleResults
            .Where(result =>
            {
                if (request.Mode == "All")
                {
                    return true;
                }

                return result.OfflineBattleMode == request.Mode;
            })
            .ToList()
            .ForEach(result =>
            {
                var detailResult = JsonConvert.DeserializeObject<Request.SaveVsmResult.PlayResultGroup>(result.FullBattleResultJson);

                if (detailResult is null)
                {
                    return;
                }
                
                var foesSize = detailResult.Foes.Count;
                if (detailResult.Foes[0].CpuFlag == 1)
                {
                    return;
                }

                var msId1 = result.Foe1MsId;

                UpsertMsBattleRecord(battleRecords, msId1, result);

                if (foesSize == 1)
                {
                    return;
                }
                
                if (detailResult.Foes[1].CpuFlag == 1)
                {
                    return;
                }
                
                var msId2 = result.Foe2MsId;

                if (msId1 == msId2)
                {
                    return;
                }
                
                UpsertMsBattleRecord(battleRecords, msId2, result);
            });

        battleRecords = battleRecords
            .OrderBy(record => record.MsId)
            .ToList();
        
        return Task.FromResult(battleRecords);
    }
    
    private void UpsertMsBattleRecord(List<MsBattleRecord> battleRecords, uint msId, OfflinePvpBattleResult result)
    {
        var msRecord = battleRecords.FirstOrDefault(x => x.MsId == msId);

        if (msRecord is null)
        {
            msRecord = new MsBattleRecord()
            {
                MsId = msId,
                WinCount = 0,
                LossCount = 0
            };

            battleRecords.Add(msRecord);
        }

        if (result.WinFlag)
        {
            msRecord.WinCount++;
        }
        else
        {
            msRecord.LossCount++;
        }
    }
}