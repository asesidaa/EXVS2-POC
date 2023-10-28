using MediatR;
using Microsoft.EntityFrameworkCore;
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
        var battleResults = context.CardProfiles
            .Include(x => x.OfflinePvpBattleResults)
            .Where(x => !x.IsNewCard)
            .SelectMany(x => x.OfflinePvpBattleResults)
            .ToList();
        
        var usage = new Usage();
        usage.BurstTypeUsage.Add(0, 0);
        usage.BurstTypeUsage.Add(1, 0);
        usage.BurstTypeUsage.Add(2, 0);
        usage.BurstTypeUsage.Add(3, 0);
        usage.BurstTypeUsage.Add(4, 0);
        
        var battleRecords = new List<MsBattleRecord>(400);
        usage.MsBattleRecords = battleRecords;
        
        battleResults
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

                var msBattleRecord = battleRecords.FirstOrDefault(battleRecord => battleRecord.MsId == battleResult.UsedMsId);

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

        return Task.FromResult(usage);
    }
}