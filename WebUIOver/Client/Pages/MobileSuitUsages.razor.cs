using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.Usage;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Usage;

namespace WebUIOver.Client.Pages;

public partial class MobileSuitUsages
{
    [Inject] 
    private IJSRuntime? _jsRuntime { get; set; }

    private readonly List<BreadcrumbItem> breadcrumbs = new();

    private string? errorMessage = null;
    private bool _loading = true;
    private MobileSuitUsageContext _mobileSuitUsageContext = new();
    private int _burstTypePieChartIndex = -1;
    private int _costPieChartIndex = -1;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem(localizer["server_mobile_suit_usages"], href: "/ServerMobileSuitUsages", disabled: false));
        
        var mobileSuitUsages = await Http.GetFromJsonAsync<List<MobileSuitUsageDto>>("/ui/usage/mobile-suit-stats/getMobileSuitUsages");
        mobileSuitUsages.ThrowIfNull();
        
        var burstUsages = await Http.GetFromJsonAsync<List<BurstUsageDto>>("/ui/usage/mobile-suit-stats/getBurstUsages");
        burstUsages.ThrowIfNull();

        _mobileSuitUsageContext.MobileSuitUsages = mobileSuitUsages;
        _mobileSuitUsageContext.BurstUsages = burstUsages;
        
        MobileSuitDataService.GetWritableMobileSuitSortedById()
            .ForEach(ms =>
            {
                if (ms.Id is < 1 or >= 50001)
                {
                    return;
                }
                
                var aggregatedMobileSuit = (MobileSuit)ms.Clone();
                
                var usageRecord = _mobileSuitUsageContext.MobileSuitUsages
                    .FirstOrDefault(usage => usage.MstMobileSuitId == aggregatedMobileSuit.Id);

                if (usageRecord is null)
                {
                    aggregatedMobileSuit.TotalBattleCount = 0;
                    aggregatedMobileSuit.WinCount = 0;
                    aggregatedMobileSuit.WinRate = 0;
                }
                else
                {
                    aggregatedMobileSuit.TotalBattleCount = usageRecord.AggregatedTotalBattle;
                    aggregatedMobileSuit.WinCount = usageRecord.AggregatedTotalWin;
                    aggregatedMobileSuit.WinRate = 100 * aggregatedMobileSuit.WinCount / aggregatedMobileSuit.TotalBattleCount;
                }
                
                _mobileSuitUsageContext.AggregetedMobileSuits.Add(aggregatedMobileSuit);

                if (aggregatedMobileSuit.Cost == 1500)
                {
                    _mobileSuitUsageContext.CostChartData[0] += aggregatedMobileSuit.TotalBattleCount;
                    _mobileSuitUsageContext.CostWinData[0] += aggregatedMobileSuit.WinCount;
                }
                
                if (aggregatedMobileSuit.Cost == 2000)
                {
                    _mobileSuitUsageContext.CostChartData[1] += aggregatedMobileSuit.TotalBattleCount;
                    _mobileSuitUsageContext.CostWinData[1] += aggregatedMobileSuit.WinCount;
                }
                
                if (aggregatedMobileSuit.Cost == 2500)
                {
                    _mobileSuitUsageContext.CostChartData[2] += aggregatedMobileSuit.TotalBattleCount;
                    _mobileSuitUsageContext.CostWinData[2] += aggregatedMobileSuit.WinCount;
                }
                
                if (aggregatedMobileSuit.Cost == 3000)
                {
                    _mobileSuitUsageContext.CostChartData[3] += aggregatedMobileSuit.TotalBattleCount;
                    _mobileSuitUsageContext.CostWinData[3] += aggregatedMobileSuit.WinCount;
                }
            });

        _mobileSuitUsageContext.BurstPieChartData = _mobileSuitUsageContext.BurstUsages
            .Select(usage => (double) usage.AggregatedTotalBattle)
            .ToArray();

        var aggregatedBurstWinRate = _mobileSuitUsageContext.BurstUsages
            .Select(usage =>
            {
                double burstWinRate = 0;

                if (usage.AggregatedTotalBattle != 0)
                {
                    burstWinRate = 100 * ((double)usage.AggregatedTotalWin) / ((double)usage.AggregatedTotalBattle);
                }

                return GetBurstChartSeries(usage.BurstType, burstWinRate);
            });

        var burstBarCharData = new List<ChartSeries>();
        burstBarCharData.Add(new ChartSeries() { Name = "", Data = new double[] { 0, 0, 0, 0, 0, 0} });
        burstBarCharData.AddRange(aggregatedBurstWinRate);
        burstBarCharData.Add(new ChartSeries() { Name = "", Data = new double[] { 0, 0, 0, 0, 0, 0} });

        _mobileSuitUsageContext.BurstBarChartData = burstBarCharData;

        if (_mobileSuitUsageContext.CostWinData[0] > 0)
        {
            _mobileSuitUsageContext.CostBarChartData[1].Data[1] =  100 * _mobileSuitUsageContext.CostWinData[0] / _mobileSuitUsageContext.CostChartData[0];
        }
        
        if (_mobileSuitUsageContext.CostWinData[1] > 0)
        {
            _mobileSuitUsageContext.CostBarChartData[2].Data[2] =  100 * _mobileSuitUsageContext.CostWinData[1] / _mobileSuitUsageContext.CostChartData[1];
        }
        
        if (_mobileSuitUsageContext.CostWinData[2] > 0)
        {
            _mobileSuitUsageContext.CostBarChartData[3].Data[3] =  100 * _mobileSuitUsageContext.CostWinData[2] / _mobileSuitUsageContext.CostChartData[2];
        }
        
        if (_mobileSuitUsageContext.CostWinData[3] > 0)
        {
            _mobileSuitUsageContext.CostBarChartData[4].Data[4] =  100 * _mobileSuitUsageContext.CostWinData[3] / _mobileSuitUsageContext.CostChartData[3];
        }
    }
    
    protected override void OnParametersSet()
    {
        if (_mobileSuitUsageContext == new MobileSuitUsageContext())
        {
            _loading = true;
            return;
        }

        if (_mobileSuitUsageContext.AggregetedMobileSuits.Count == 0)
        {
            _loading = true;
            return;
        }
        
        _loading = false;
    }

    protected ChartSeries GetBurstChartSeries(uint burstType, double burstWinRate)
    {
        if (burstType == 1)
        {
            return new ChartSeries() { Name = "Fighting", Data = [0, 0, burstWinRate, 0, 0, 0] };
        }

        if (burstType == 2)
        {
            return new ChartSeries() { Name = "Shooting", Data = [0, 0, 0, burstWinRate, 0, 0] };
        }

        if (burstType == 3)
        {
            return new ChartSeries() { Name = "Vertical", Data = [0, 0, 0, 0, burstWinRate, 0] };
        }

        return new ChartSeries() { Name = "Covering", Data = [0, burstWinRate, 0, 0, 0, 0] };
    }
}