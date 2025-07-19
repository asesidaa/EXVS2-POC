using MudBlazor;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Usage;

namespace WebUIOver.Client.Context.Usage;

public class MobileSuitUsageContext
{
    public List<MobileSuitUsageDto> MobileSuitUsages { get; set; } = new();
    public List<MobileSuit> AggregetedMobileSuits { get; set; } = new();
    public int[] TablePageSizeOptions { get; set; }= { 25, 50, 100 };
    public List<BurstUsageDto> BurstUsages { get; set; } = new();
    public string[] BurstChartLabels = ["Covering", "Fighting", "Shooting", "Vertical"];
    public ChartOptions BurstPieChartOptions { get; set; } = new()
    {
        ChartPalette = ["#92cbae", "#e93500", "#004790", "#690091"]
    };
    public double[] BurstPieChartData { get; set; } = [0, 0, 0, 0];
    public ChartOptions BurstBarChartOptions { get; set; } = new()
    {
        ShowLegend = false,
        ShowLegendLabels = false,
        ChartPalette = ["#000000", "#92cbae", "#e93500", "#004790", "#690091", "#000000"]
    };
    public string[] BurstBarChartXAxisLabels { get; set; }= { "", "Covering", "Fighting", "Shooting", "Vertical", "" };
    public List<ChartSeries> BurstBarChartData = new()
    {
        new ChartSeries() { Name = "", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "Covering", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "Fighting", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "Shooting", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "Vertical", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "", Data = new double[] { 0, 0, 0, 0, 0, 0} }
    };
    
    public double[] CostChartData { get; set; } = [0, 0, 0, 0];
    public double[] CostWinData { get; set; } = [0, 0, 0, 0];
    public string[] CostChartLabels = ["1500", "2000", "2500", "3000"];
    public ChartOptions CostPieChartOptions { get; set; } = new()
    {
        ChartPalette = ["#99ff00", "#fff11d", "#ff8900", "#f7003a"]
    };
    
    public ChartOptions CostBarChartOptions { get; set; } = new()
    {
        ShowLegend = false,
        ShowLegendLabels = false,
        ChartPalette = ["#000000", "#99ff00", "#fff11d", "#ff8900", "#f7003a", "#000000"]
    };
    public string[] CostBarChartXAxisLabels { get; set; }= { "", "1500", "2000", "2500", "3000", "" };
    public List<ChartSeries> CostBarChartData = new()
    {
        new ChartSeries() { Name = "", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "1500", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "2000", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "2500", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "3000", Data = new double[] { 0, 0, 0, 0, 0, 0} },
        new ChartSeries() { Name = "", Data = new double[] { 0, 0, 0, 0, 0, 0} }
    };
}