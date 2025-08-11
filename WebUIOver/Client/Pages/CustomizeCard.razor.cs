using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using WebUIOver.Client.Command.CustomizeCard.Fill;
using WebUIOver.Client.Command.CustomizeCard.Save;
using WebUIOver.Client.Context.CustomizeCard;

namespace WebUIOver.Client.Pages;

public partial class CustomizeCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;
    
    [Inject]
    private IConfiguration Configuration { get; set; } = null!;

    [Inject]
    private IJSRuntime? JsRuntime { get; set; }
    
    private bool EnableImagePreview { get; set; } = false;
    private string? _errorMessage = null;
    
    private readonly List<BreadcrumbItem> _breadcrumbs = new();
    
    // Init Context
    private readonly CustomizeCardContext _customizeCardContext = new();
    private readonly ButtonStatusContext _buttonStatusContext = new();
    private readonly ProgressContext _progressContext = new();
    
    [Inject] private BasicProfileFiller _basicProfileFiller { get; set; } = null!;
    [Inject] private PlayerLevelProfileFiller _playerLevelProfileFiller { get; set; } = null!;
    [Inject] private NaviProfileFiller _naviProfileFiller { get; set; } = null!;
    
    [Inject] private MsSkillGroupAggregator _msSkillGroupAggregator { get; set; } = null!;
    [Inject] private FavouriteMobileSuitFiller _favouriteMobileSuitFiller { get; set; } = null!;
    [Inject] private MsAlternativeCostumeAggregator _msAlternativeCostumeAggregator { get; set; } = null!;
    [Inject] private MsAlternativeSkinAggregator _msAlternativeSkinAggregator { get; set; } = null!;
    [Inject] private CustomMessageGroupFiller _customMessageGroupFiller { get; set; } = null!;
    [Inject] private DefaultStickerSettingFiller _defaultStickerSettingFiller { get; set; } = null!;
    [Inject] private MobileSuitStickersSettingFiller _mobileSuitStickersSettingFiller { get; set; } = null!;
    [Inject] private CpuTriadPartnerFiller _cpuTriadPartnerFiller { get; set; } = null!;
    [Inject] private TeamResponseFiller _teamResponseFiller { get; set; } = null!;
    [Inject] private GamepadConfigFiller _gamepadConfigFiller { get; set; } = null!;
    [Inject] private TrainingProfileFiller _trainingProfileFiller { get; set; } = null!;
    
    [Inject] private BasicProfileSaver _basicProfileSaver { get; set; } = null!;
    [Inject] private NaviProfileSaver _naviProfileSaver { get; set; } = null!;
    
    [Inject] private NaviCostumeSaver _naviCostumeSaver { get; set; } = null!;
    [Inject] private FavouriteMobileSuitSaver _favouriteMobileSuitSaver { get; set; } = null!;
    [Inject] private MsCostumeSaver _msCostumeSaver { get; set; } = null!;
    [Inject] private MsSkinSaver _msSkinSaver { get; set; } = null!;
    [Inject] private CustomMessageGroupSaver _customMessageGroupSaver { get; set; } = null!;
    [Inject] private DefaultStickerSettingSaver _defaultStickerSettingSaver { get; set; } = null!;
    [Inject] private MobileSuitStickerSettingsSaver _mobileSuitStickerSettingsSaver { get; set; } = null!;
    [Inject] private CpuTriadPartnerSaver _cpuTriadPartnerSaver { get; set; } = null!;
    [Inject] private TeamSaver _teamSaver { get; set; } = null!;
    [Inject] private GamepadConfigSaver _gamepadConfigSaver { get; set; } = null!;
    [Inject] private TrainingProfileSaver _TrainingProfileSaver { get; set; } = null!;
    
    private bool _loading = true;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        EnableImagePreview = Configuration.GetValue<bool>("EnableImagePreview");
        
        _breadcrumbs.Add(new BreadcrumbItem("Menu", href: $"Cards/SingularCardMenu/{ChipId}", disabled: false));

        AccessCode = await JsRuntime.InvokeAsync<string>("accessCode.get");

        if (string.IsNullOrEmpty(AccessCode))
        {
            Snackbar.Add($"Invalid access code!", Severity.Error);
            return;
        }

        _customizeCardContext.AccessCode = AccessCode;
        _customizeCardContext.ChipId = ChipId;
        
        await _basicProfileFiller.Fill(_customizeCardContext);
        await _playerLevelProfileFiller.Fill(_customizeCardContext);
        await _naviProfileFiller.Fill(_customizeCardContext);
        await _msSkillGroupAggregator.Fill(_customizeCardContext);
        await _favouriteMobileSuitFiller.Fill(_customizeCardContext);
        await _msAlternativeCostumeAggregator.Fill(_customizeCardContext);
        await _msAlternativeSkinAggregator.Fill(_customizeCardContext);
        await _customMessageGroupFiller.Fill(_customizeCardContext);
        await _defaultStickerSettingFiller.Fill(_customizeCardContext);
        await _mobileSuitStickersSettingFiller.Fill(_customizeCardContext);
        await _cpuTriadPartnerFiller.Fill(_customizeCardContext);
        await _teamResponseFiller.Fill(_customizeCardContext);
        await _gamepadConfigFiller.Fill(_customizeCardContext);
        await _trainingProfileFiller.Fill(_customizeCardContext);
    }
    
    protected override void OnParametersSet()
    {
        _loading = false;
    }
    
    private async Task SaveAll()
    {
        _buttonStatusContext.SaveAllButtonDisabled = true;
        _progressContext.HideSaveAllProgress = "visible";
        StateHasChanged();

        await _basicProfileSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        
        await _naviProfileSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _naviCostumeSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _favouriteMobileSuitSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _msCostumeSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _msSkinSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _customMessageGroupSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _defaultStickerSettingSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _mobileSuitStickerSettingsSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _cpuTriadPartnerSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _teamSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _gamepadConfigSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        await _TrainingProfileSaver.Save(_customizeCardContext, _progressContext, Snackbar, StateHasChanged);
        
        _progressContext.HideSaveAllProgress = "invisible";
        _buttonStatusContext.SaveAllButtonDisabled = false;
        StateHasChanged();
    }
}