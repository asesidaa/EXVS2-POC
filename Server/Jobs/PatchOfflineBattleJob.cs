using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Quartz;
using Server.Models.Cards;
using Server.Models.Config;
using Server.Persistence;
using WebUI.Shared.Dto.Enum;

namespace Server.Jobs;

public class PatchOfflineBattleJob : IJob
{
    private readonly ILogger<PatchOfflineBattleJob> _logger;
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;

    public PatchOfflineBattleJob(ILogger<PatchOfflineBattleJob> logger, ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _logger = logger;
        _context = context;
        _config = options.Value;
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        if (!_config.NeedToDataPatchOfflineData)
        {
            _logger.LogInformation("Skipped Data Patch Job");
            return Task.CompletedTask;
        }
        
        _logger.LogInformation("Running Data Patch Job");
        
        _context.OfflinePvpBattleResults
            .ToList()
            .ForEach(offlinePvpBattleResult =>
            {
                var detailResult = JsonConvert.DeserializeObject<Request.SaveVsmResult.PlayResultGroup>(offlinePvpBattleResult.FullBattleResultJson);

                if (detailResult is null)
                {
                    return;
                }

                offlinePvpBattleResult.UsedBurstType = detailResult.BurstType;

                if (detailResult.Partner is not null)
                {
                    if (detailResult.Partner.CpuFlag == 0)
                    {
                        offlinePvpBattleResult.PartnerIndicator = PlayerIndicator.Player;
                    }
                    else
                    {
                        offlinePvpBattleResult.PartnerIndicator = PlayerIndicator.Cpu;
                    }
                }
                else
                {
                    offlinePvpBattleResult.PartnerIndicator = PlayerIndicator.Discarded;
                }
                
                if (detailResult.Foes.Count == 0)
                {
                    return;
                }
                
                var foe1 = detailResult.Foes.ElementAt(0);
                
                if (foe1 is not null)
                {
                    if (foe1.CpuFlag == 0)
                    {
                        offlinePvpBattleResult.Foe1Indicator = PlayerIndicator.Player;
                    }
                    else
                    {
                        offlinePvpBattleResult.Foe1Indicator = PlayerIndicator.Cpu;
                    }
                }
                
                if (detailResult.Foes.Count == 1)
                {
                    offlinePvpBattleResult.Foe2Indicator = PlayerIndicator.Discarded;
                    return;
                }
                
                var foe2 = detailResult.Foes.ElementAt(1);
                
                if (foe2 is not null)
                {
                    if (foe2.CpuFlag == 0)
                    {
                        offlinePvpBattleResult.Foe2Indicator = PlayerIndicator.Player;
                    }
                    else
                    {
                        offlinePvpBattleResult.Foe2Indicator = PlayerIndicator.Cpu;
                    }
                }
            });

        _context.SaveChanges();
        
        _logger.LogInformation("End Data Patch Job");
        
        return Task.CompletedTask;
    }
}