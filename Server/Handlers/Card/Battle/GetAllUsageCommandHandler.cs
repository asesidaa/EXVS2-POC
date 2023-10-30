using MediatR;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Battle;

public record GetAllUsageCommand(string Mode) : IRequest<Usage>;

public class GetAllUsageCommandHandler : IRequestHandler<GetAllUsageCommand, Usage>
{
    private readonly ServerDbContext context;
    
    public GetAllUsageCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<Usage> Handle(GetAllUsageCommand request, CancellationToken cancellationToken)
    {
        var usage = new Usage();
        usage.BurstTypeUsage.Add(0, 0);
        usage.BurstTypeUsage.Add(1, 0);
        usage.BurstTypeUsage.Add(2, 0);
        usage.BurstTypeUsage.Add(3, 0);
        usage.BurstTypeUsage.Add(4, 0);
        
        var battleRecords = new List<MsBattleRecord>(400);
        usage.MsBattleRecords = battleRecords;
        
        var consolidatedData = context.Snapshots
            .FirstOrDefault(snapshot => snapshot.SnapshotType == "OfflineConsolidation");

        if (consolidatedData is null)
        {
            SetByRawData(request, usage, battleRecords);
            return Task.FromResult(usage);
        }
        
        var fullUsageSnapshot = JsonConvert.DeserializeObject<FullUsageSnapshot>(consolidatedData.SnapshotData);
        
        if (fullUsageSnapshot is null)
        {
            SetByRawData(request, usage, battleRecords);
            return Task.FromResult(usage);
        }
        
        fullUsageSnapshot.FullBurstTypeUsages.ForEach(burstTypeUsage =>
        {
            var burstTypeId = burstTypeUsage.BurstTypeId;
            
            if (request.Mode == "All")
            {
                usage.BurstTypeUsage[burstTypeId] = burstTypeUsage.OfflineTeamUsage + burstTypeUsage.OfflineShuffleUsage;
            }
            else if (request.Mode == "Shuffle")
            {
                usage.BurstTypeUsage[burstTypeId] = burstTypeUsage.OfflineShuffleUsage;
            }
            else
            {
                usage.BurstTypeUsage[burstTypeId] = burstTypeUsage.OfflineTeamUsage;
            }
        });
        
        fullUsageSnapshot.FullMsBattleRecords.ForEach(conBattleRecord =>
        {
            var msUsage = battleRecords
                .FirstOrDefault(battleRecord => conBattleRecord.MsId == battleRecord.MsId);

            if (msUsage is null)
            {
                msUsage = new MsBattleRecord();
                msUsage.MsId = conBattleRecord.MsId;
                usage.MsBattleRecords.Add(msUsage);
            }
            
            if (request.Mode == "All")
            {
                msUsage.WinCount += conBattleRecord.OfflineShuffleWinCount + conBattleRecord.OfflineTeamWinCount;
                msUsage.LossCount += conBattleRecord.OfflineShuffleLossCount + conBattleRecord.OfflineTeamLossCount;
            }
            else if (request.Mode == "Shuffle")
            {
                msUsage.WinCount += conBattleRecord.OfflineShuffleWinCount;
                msUsage.LossCount += conBattleRecord.OfflineShuffleLossCount;
            }
            else
            {
                msUsage.WinCount += conBattleRecord.OfflineTeamWinCount;
                msUsage.LossCount += conBattleRecord.OfflineTeamLossCount;
            }
        });
        
        usage.MsBattleRecords = usage.MsBattleRecords
            .OrderBy(record => record.MsId)
            .ToList();
        
        return Task.FromResult(usage);
    }

    private void SetByRawData(GetAllUsageCommand request, Usage usage, List<MsBattleRecord> battleRecords)
    {
        context.OfflinePvpBattleResults
            .ToList()
            .Where(result =>
            {
                if (request.Mode == "All")
                {
                    return true;
                }

                return result.OfflineBattleMode == request.Mode;
            })
            .ToList()
            .ForEach(battleResult =>
            {
                var detailResult = JsonConvert.DeserializeObject<Request.SaveVsmResult.PlayResultGroup>(battleResult.FullBattleResultJson);

                if (detailResult is null)
                {
                    return;
                }

                usage.BurstTypeUsage[detailResult.BurstType]++;

                var msBattleRecord =
                    battleRecords.FirstOrDefault(battleRecord => battleRecord.MsId == battleResult.UsedMsId);

                if (msBattleRecord is null)
                {
                    msBattleRecord = new MsBattleRecord();
                    msBattleRecord.MsId = battleResult.UsedMsId;
                    battleRecords.Add(msBattleRecord);
                }

                if (battleResult.WinFlag)
                {
                    msBattleRecord.WinCount++;
                }
                else
                {
                    msBattleRecord.LossCount++;
                }
            });
        
        usage.MsBattleRecords = usage.MsBattleRecords
            .OrderBy(record => record.MsId)
            .ToList();
    }
}