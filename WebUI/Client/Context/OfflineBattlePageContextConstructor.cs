using System.Net.Http.Json;
using Throw;
using WebUI.Client.Services;
using WebUI.Shared.Context;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Response;

namespace WebUI.Client.Context;

public class OfflineBattlePageContextConstructor : IBattlePageContextConstructor
{
    private readonly HttpClient _httpClient;
    private readonly string _accessCode;
    private readonly string _chipId;
    private readonly string _mode;
    private readonly IDataService _dataService;
    
    public OfflineBattlePageContextConstructor(HttpClient httpClient, IDataService dataService, string accessCode, string chipId, string mode)
    {
        _httpClient = httpClient;
        _dataService = dataService;
        _accessCode = accessCode;
        _chipId = chipId;
        _mode = mode;
    }
    
    public async Task<BattlePageContext> Construct()
    {
        var battlePageContext = new BattlePageContext();
        
        var profileResult = await _httpClient.GetFromJsonAsync<BasicProfile>($"/card/getBasicDisplayProfile/{_accessCode}/{_chipId}");
        profileResult.ThrowIfNull();
        
        var msSkillGroup = await _httpClient.GetFromJsonAsync<List<MsSkillGroup>>($"/card/getUsedMobileSuitData/{_accessCode}/{_chipId}");
        msSkillGroup.ThrowIfNull();
        
        var favouriteResult = await _httpClient.GetFromJsonAsync<List<FavouriteMs>>($"/card/getAllFavouriteMs/{_accessCode}/{_chipId}");
        favouriteResult.ThrowIfNull();
        
        var usageStat = await _httpClient.GetFromJsonAsync<Usage>($"/battle-analysis/getSelfUsage/{_accessCode}/{_chipId}/{_mode}");
        usageStat.ThrowIfNull();
        
        var battleRecords = await _httpClient.GetFromJsonAsync<List<MsBattleRecord>>($"/battle-analysis/getAgainstMsWinLossRecord/{_accessCode}/{_chipId}/{_mode}");
        battleRecords.ThrowIfNull();
        
        battlePageContext.BasicProfile = profileResult;
        battlePageContext.UsageStat = usageStat;
        battlePageContext.MsBattleRecords = battleRecords;
        
        ConsolidateBasicData(battlePageContext, usageStat);
        
        _dataService.GetWritableMobileSuitSortedById()
            .ForEach(writableMs =>
            {
                var favouriteMs = favouriteResult.FirstOrDefault(fav => fav.MsId == writableMs.Id);
                writableMs.IsFavouriteMs = favouriteMs is not null;
                ConsolidateWinLossRecord(battlePageContext.UsageStat.MsBattleRecords, writableMs);
                ConsolidateAgainstRecord(battleRecords, writableMs);
                ConsolidateMasteryData(writableMs, msSkillGroup);
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

    private void ConsolidateMasteryData(MobileSuit writableMs, List<MsSkillGroup>? msSkillGroup)
    {
        if (writableMs.Id == 0)
        {
            writableMs.MasteryPoint = -1;
            return;
        }

        var msData = msSkillGroup
            .FirstOrDefault(msSkill => msSkill.MstMobileSuitId == writableMs.Id);

        if (msData is null)
        {
            writableMs.MasteryDomain = _dataService.GetMsFamiliaritySortedById().First();
            return;
        }

        writableMs.MasteryPoint = (int)msData.MsUsedNum;
        writableMs.MasteryDomain = _dataService.GetMsFamiliaritySortedById()
            .Reverse()
            .First(msFamiliarity => msData.MsUsedNum >= msFamiliarity.MinimumPoint);
    }

    private void ConsolidateBasicData(BattlePageContext battlePageContext, Usage usageStat)
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

    private void PrepareCostData(BattlePageContext battlePageContext, MobileSuit mobileSuit, uint battleCount)
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