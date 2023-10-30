using Newtonsoft.Json;
using Quartz;
using Server.Models.Cards;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Jobs;

public class ConsolidateServerOfflineSnapshotJob : IJob
{
    private readonly ILogger<ConsolidateServerOfflineSnapshotJob> _logger;
    private readonly ServerDbContext _context;
    
    public ConsolidateServerOfflineSnapshotJob(ILogger<ConsolidateServerOfflineSnapshotJob> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Running ConsolidateServerOfflineSnapshotJob");
        var consolidatedData = _context.Snapshots
            .FirstOrDefault(snapshot => snapshot.SnapshotType == "OfflineConsolidation");

        if (consolidatedData is null)
        {
            consolidatedData = new Snapshot()
            {
                SnapshotType = "OfflineConsolidation",
                SnapshotData = ""
            };
            
            _context.Snapshots.Add(consolidatedData);
        }
        
        var fullUsageSnapshot = JsonConvert.DeserializeObject<FullUsageSnapshot>(consolidatedData.SnapshotData);

        if (fullUsageSnapshot is null)
        {
            _logger.LogInformation("Init Snapshot");
            fullUsageSnapshot = new FullUsageSnapshot()
            {
                CurrentBattleCount = 0,
                FullMsBattleRecords = new(400),
                FullBurstTypeUsages = new List<FullBurstTypeUsage>(5)
            };
            
            for (uint i = 0; i < 5; i++)
            {
                fullUsageSnapshot.FullBurstTypeUsages.Add(new FullBurstTypeUsage()
                {
                    BurstTypeId = i,
                    OfflineTeamUsage = 0,
                    OfflineShuffleUsage = 0
                });
            }
        }
        
        
        var battleList = _context.OfflinePvpBattleResults
            .Where(battleRecord => battleRecord.Id > fullUsageSnapshot.CurrentBattleCount)
            .ToList();
        
        _logger.LogInformation("No of record that will be consolidated = {}", battleList.Count);
            
        battleList.ForEach(battleResult =>
            {
                var battleType = battleResult.OfflineBattleMode;
                var win = battleResult.WinFlag;

                var targetBurstType = fullUsageSnapshot.FullBurstTypeUsages
                    .FirstOrDefault(burstType => burstType.BurstTypeId == battleResult.UsedBurstType);

                if (targetBurstType is null)
                {
                    targetBurstType = new FullBurstTypeUsage()
                    {
                        BurstTypeId = battleResult.UsedBurstType,
                        OfflineTeamUsage = 0,
                        OfflineShuffleUsage = 0
                    };
                    
                    fullUsageSnapshot.FullBurstTypeUsages.Add(targetBurstType);
                }
                
                var msBattleRecord = fullUsageSnapshot.FullMsBattleRecords
                    .FirstOrDefault(battleRecord => battleRecord.MsId == battleResult.UsedMsId);

                if (msBattleRecord is null)
                {
                    msBattleRecord = new FullMsBattleRecord()
                    {
                        MsId = battleResult.UsedMsId,
                        OfflineShuffleWinCount = 0,
                        OfflineShuffleLossCount = 0,
                        OfflineTeamWinCount = 0,
                        OfflineTeamLossCount = 0
                    };
                    fullUsageSnapshot.FullMsBattleRecords.Add(msBattleRecord);
                }
                
                if (battleType == "Shuffle")
                {
                    targetBurstType.OfflineShuffleUsage += 1;

                    if (win)
                    {
                        msBattleRecord.OfflineShuffleWinCount += 1;
                    }
                    else
                    {
                        msBattleRecord.OfflineShuffleLossCount += 1;
                    }
                }
                else
                {
                    targetBurstType.OfflineTeamUsage += 1;
                    
                    if (win)
                    {
                        msBattleRecord.OfflineTeamWinCount += 1;
                    }
                    else
                    {
                        msBattleRecord.OfflineTeamLossCount += 1;
                    }
                }
            });

        fullUsageSnapshot.CurrentBattleCount += (uint) battleList.Count;
        
        _logger.LogInformation("Updated snapshot counter = {}", fullUsageSnapshot.CurrentBattleCount);

        consolidatedData.SnapshotData = JsonConvert.SerializeObject(fullUsageSnapshot);

        _context.SaveChanges();
        
        _logger.LogInformation("ConsolidateServerOfflineSnapshotJob, Consolidate Finished...");
        
        return Task.CompletedTask;
    }
}