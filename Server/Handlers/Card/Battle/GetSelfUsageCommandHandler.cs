using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;

namespace Server.Handlers.Card.Battle;

public record GetSelfUsageCommand(string AccessCode, string ChipId, string Mode) : IRequest<Usage>;

public class GetSelfUsageCommandHandler : IRequestHandler<GetSelfUsageCommand, Usage>
{
    private readonly ServerDbContext context;
    
    public GetSelfUsageCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }

    public Task<Usage> Handle(GetSelfUsageCommand request, CancellationToken cancellationToken)
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
            return Task.FromResult(new Usage());
        }

        var usage = new Usage();
        usage.BurstTypeUsage.Add(1, 0);
        usage.BurstTypeUsage.Add(2, 0);
        usage.BurstTypeUsage.Add(3, 0);
        usage.BurstTypeUsage.Add(4, 0);
        usage.BurstTypeUsage.Add(5, 0);
        
        var battleRecords = new List<MsBattleRecord>(400);
        usage.MsBattleRecords = battleRecords;
        
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

        return Task.FromResult(usage);
    }
}