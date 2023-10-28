using System.Net.Http.Json;
using Throw;
using WebUI.Client.Services;
using WebUI.Shared.Context;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Response;

namespace WebUI.Client.Context;

public class ServerBattlePageContextConstructor : IServerBattlePageContextConstructor
{
    private readonly HttpClient _httpClient;
    private readonly IDataService _dataService;
    private readonly string _mode;
    
    public ServerBattlePageContextConstructor(HttpClient httpClient, IDataService dataService, string mode)
    {
        _httpClient = httpClient;
        _dataService = dataService;
        _mode = mode;
    }
    
    public async Task<ServerBattlePageContext> Construct()
    {
        var battlePageContext = new ServerBattlePageContext();
        
        var usageStat = await _httpClient.GetFromJsonAsync<Usage>($"/battle-analysis/getAllUsage/{_mode}");
        usageStat.ThrowIfNull();
        
        // var battleRecords = await _httpClient.GetFromJsonAsync<List<MsBattleRecord>>($"/battle-analysis/getAgainstMsWinLossRecord/{_accessCode}/{_chipId}/{_mode}");
        // battleRecords.ThrowIfNull();
        
        battlePageContext.UsageStat = usageStat;
        // battlePageContext.MsBattleRecords = battleRecords;
        
        ConsolidateBasicData(battlePageContext, usageStat);
        
        _dataService.GetWritableMobileSuitSortedById()
            .ForEach(writableMs =>
            {
                ConsolidateWinLossRecord(battlePageContext.UsageStat.MsBattleRecords, writableMs);
                // ConsolidateAgainstRecord(battleRecords, writableMs);
            });

        return battlePageContext;
    }
    
    private void ConsolidateWinLossRecord(List<MsBattleRecord>? battleRecords, MobileSuit writableMs)
    {
        var record = battleRecords.FirstOrDefault(battleRecord => battleRecord.MsId == writableMs.Id);

        if (record is null)
        {
            return;
        }

        var totalCount = record.WinCount + record.LossCount;
        writableMs.TotalBattleCount = totalCount;
        writableMs.WinCount = record.WinCount;
        
        if (totalCount > 0)
        {
            writableMs.WinRate = 100 * writableMs.WinCount / writableMs.TotalBattleCount;
        }
    }

    private void ConsolidateAgainstRecord(List<MsBattleRecord>? battleRecords, MobileSuit writableMs)
    {
        var againstRecord = battleRecords.FirstOrDefault(battleRecord => battleRecord.MsId == writableMs.Id);

        if (againstRecord is null)
        {
            return;
        }

        var againstTotalCount = againstRecord.WinCount + againstRecord.LossCount;
        writableMs.TotalAgainstBattleCount = againstTotalCount;
        writableMs.WinAgainstCount = againstRecord.WinCount;
        
        if (againstTotalCount > 0)
        {
            writableMs.WinAgainstRate = 100 * writableMs.WinAgainstCount / writableMs.TotalAgainstBattleCount;
        }
    }
    
    private void ConsolidateBasicData(ServerBattlePageContext battlePageContext, Usage usageStat)
    {
        battlePageContext.CostUsage.Add(0, 0);
        battlePageContext.CostUsage.Add(1, 0);
        battlePageContext.CostUsage.Add(2, 0);
        battlePageContext.CostUsage.Add(3, 0);

        IReadOnlyList<MobileSuit> mobileSuits = _dataService.GetMobileSuitSortedById();

        usageStat.MsBattleRecords
            .ToList()
            .ForEach(msBattleRecord =>
            {
                if (msBattleRecord.MsId == 0)
                {
                    return;
                }
                
                var mobileSuit = mobileSuits.FirstOrDefault(ms => ms.Id == msBattleRecord.MsId);

                if (mobileSuit is null)
                {
                    return;
                }

                var battleCount = msBattleRecord.WinCount + msBattleRecord.LossCount;
                battlePageContext.TotalBattleCount += battleCount;
                battlePageContext.TotalWinCount += msBattleRecord.WinCount;

                PrepareCostData(battlePageContext, mobileSuit, battleCount);
            });
    }

    private void PrepareCostData(ServerBattlePageContext battlePageContext, MobileSuit mobileSuit, uint battleCount)
    {
        if (mobileSuit.Cost == 1500)
        {
            battlePageContext.CostUsage[0] += battleCount;
        }

        if (mobileSuit.Cost == 2000)
        {
            battlePageContext.CostUsage[1] += battleCount;
        }

        if (mobileSuit.Cost == 2500)
        {
            battlePageContext.CostUsage[2] += battleCount;
        }

        if (mobileSuit.Cost == 3000)
        {
            battlePageContext.CostUsage[3] += battleCount;
        }
    }
}