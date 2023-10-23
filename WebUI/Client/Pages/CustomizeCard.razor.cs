using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Throw;
using WebUI.Client.Pages.Dialogs;
using WebUI.Client.Validator;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;

namespace WebUI.Client.Pages;

public partial class CustomizeCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;
    
    [Inject]
    private IConfiguration configuration { get; set; }
    
    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }
    
    [Inject]
    private INameValidator _nameValidator { get; set; }

    private bool EnableImagePreview { get; set; } = false;

    private BasicProfile _basicProfile = null!;
    private NaviProfile _naviProfile  = null!;
    private ObservableCollection<FavouriteMs> _favouriteMs  = new();
    private ObservableCollection<MobileSuitWithSkillGroup> _mobileSuitsSkillGroups = new();
    private ObservableCollection<NaviWithNavigatorGroup> _naviObservableCollection = new();
    private CpuTriadPartner cpuTriadPartner = null;
    private GamepadConfig _gamepadConfig = null;
    private CustomMessageGroupSetting _customMessageGroupSetting = null!;
    private List<Team> _tagTeams = new();
    
    MudForm _teamNameForm;
    MudForm _messageForm;

    private string? errorMessage = null;

    private readonly int maximumFavouriteMs = 6;

    private bool SaveAllButtonDisabled { get; set; } = false;
    private string HideSaveAllProgress { get; set; } = "invisible";
    private string HideProfileProgress { get; set; } = "invisible";
    private string HideNaviProgress { get; set; } = "invisible";
    private string HideFavMsProgress { get; set; } = "invisible";
    private string HideMsCostumeProgress { get; set; } = "invisible";
    private string HideNaviCostumeProgress { get; set; } = "invisible";
    private string HideTeamTagsProgress { get; set; } = "invisible";
    private string _msCostumeSearchString { get; set; }
    private string _naviCostumeSearchString { get; set; }

    private readonly int[] _pageSizeOptions = { 5, 10, 25, 50, 100 };

    private string HideTriadCpuPartnerProgress { get; set; } = "invisible";
    private string HideCustomizeCommentProgress { get; set; } = "invisible";
    private string HideGamepadConfigProgress { get; set; } = "invisible";
    private string HideCommunicationMessageProgress { get; set; } = "invisible";

    private IdValuePair? SelectedTriadSkill1 { get; set; }
    private IdValuePair? SelectedTriadSkill2 { get; set; }
    private IdValuePair? SelectedTriadTeamBanner { get; set; }
    private CustomizeComment? CustomizeComment { get; set; }
    
    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new BreadcrumbItem("Cards", href: "/Cards"),
    };

    private static readonly DialogOptions OPTIONS = new()
    {
        CloseOnEscapeKey = false,
        DisableBackdropClick = true,
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraExtraLarge
    };

    private static readonly DialogOptions LightOptions = new()
    {
        CloseOnEscapeKey = false,
        DisableBackdropClick = true
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        EnableImagePreview = configuration.GetValue<bool>("EnableImagePreview");
        
        breadcrumbs.Add(new BreadcrumbItem($"Card: {ChipId}", href: null, disabled: true));
        breadcrumbs.Add(new BreadcrumbItem("Option", href: $"/Cards/Customize/{ChipId}", disabled: false));

        AccessCode = await _jsRuntime.InvokeAsync<string>("accessCode.get");

        if (string.IsNullOrEmpty(AccessCode))
        {
            Snackbar.Add($"Invalid access code!", Severity.Error);
        }
        else
        {
            var profileResult = await Http.GetFromJsonAsync<BasicProfile>($"/card/getBasicDisplayProfile/{AccessCode}/{ChipId}");
            profileResult.ThrowIfNull();

            var naviResult = await Http.GetFromJsonAsync<NaviProfile>($"/card/getNaviProfile/{AccessCode}/{ChipId}");
            naviResult.ThrowIfNull();

            var favouriteResult = await Http.GetFromJsonAsync<List<FavouriteMs>>($"/card/getAllFavouriteMs/{AccessCode}/{ChipId}");
            favouriteResult.ThrowIfNull();

            var cpuTriadPartnerResult = await Http.GetFromJsonAsync<CpuTriadPartner>($"/card/getCpuTriadPartner/{AccessCode}/{ChipId}");
            cpuTriadPartnerResult.ThrowIfNull();

            var customizeCommentResult = await Http.GetFromJsonAsync<CustomizeComment>($"/card/getCustomizeComment/{AccessCode}/{ChipId}");
            customizeCommentResult.ThrowIfNull();

            var msSkillGroup = await Http.GetFromJsonAsync<List<MsSkillGroup>>($"/card/getUsedMobileSuitData/{AccessCode}/{ChipId}");
            msSkillGroup.ThrowIfNull();

            var gamepadConfig = await Http.GetFromJsonAsync<GamepadConfig>($"/card/getGamepadConfig/{AccessCode}/{ChipId}");
            gamepadConfig.ThrowIfNull();

            var customMessageGroupSetting = await Http.GetFromJsonAsync<CustomMessageGroupSetting>($"/card/getCustomMessageGroupSetting/{AccessCode}/{ChipId}");
            customMessageGroupSetting.ThrowIfNull();
            
            var tagTeamResults = await Http.GetFromJsonAsync<List<Team>>($"/card/getTeams/{AccessCode}/{ChipId}");
            tagTeamResults.ThrowIfNull();

            //var json = System.Text.Json.JsonSerializer.Serialize(naviResult);
            //Logger.LogInformation($"{json}");

            // assign costume selection id from ms skill group to the mobile suit list
            var mobileSuitList = DataService.GetMobileSuitSortedById().Select(x => new MobileSuitWithSkillGroup { MobileSuit = x }).ToList();
            
            DataService.GetWritableMobileSuitSortedById()
                .ForEach(writableMs =>
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
                        writableMs.MasteryDomain = DataService.GetMsFamiliaritySortedById().First();
                        return;
                    }

                    writableMs.MasteryPoint = (int) msData.MsUsedNum;
                    writableMs.MasteryDomain = DataService.GetMsFamiliaritySortedById()
                        .Reverse()
                        .First(msFamiliarity => msData.MsUsedNum >= msFamiliarity.MinimumPoint);
                });
            
            var msWithAltCostumes = mobileSuitList
                .Where(x => x.MobileSuit.Costumes != null && x.MobileSuit.Costumes.Count > 0)
                .GroupJoin(
                    msSkillGroup,
                    firstItem => firstItem.MobileSuit.Id,
                    secondItem => secondItem.MstMobileSuitId,
                    (firstItem, matchingSecondItems) => new
                    {
                        FirstItem = firstItem,
                        MatchingSecondItem = matchingSecondItems.FirstOrDefault()
                    })
                .Select(joinedItem =>
                {
                    if (joinedItem.MatchingSecondItem != null)
                    {
                        joinedItem.FirstItem.SkillGroup = joinedItem.MatchingSecondItem;
                    }
                    return joinedItem.FirstItem;
                })
                .ToList();

            var naviList = DataService.GetNavigatorSortedById().Select(navigator => new NaviWithNavigatorGroup()
            {
                Navigator = navigator
            }).ToList();
            
            DataService.GetWritableNavigatorSortedById()
                .ForEach(writableNavi =>
                {
                    if (writableNavi.Id == 0)
                    {
                        writableNavi.ClosenessPoint = -1;
                        return;
                    }
                    
                    var naviData = naviResult.UserNavis
                        .FirstOrDefault(userNavi => userNavi.Id == writableNavi.Id);

                    if (naviData is null)
                    {
                        writableNavi.FamiliarityDomain = DataService.GetNaviFamiliaritySortedById().First();
                        return;
                    }

                    writableNavi.ClosenessPoint = (int) naviData.Familiarity;
                    writableNavi.FamiliarityDomain = DataService.GetNaviFamiliaritySortedById()
                        .Reverse()
                        .First(naviFamiliarity => naviData.Familiarity >= naviFamiliarity.MinimumPoint);
                });

            var naviWithAltCostumes = naviList
                .Where(x => x.Navigator.Costumes != null && x.Navigator.Costumes.Count > 0)
                .GroupJoin(
                    naviResult.UserNavis,
                    firstItem => firstItem.Navigator.Id,
                    secondItem => secondItem.Id,
                    (firstItem, matchingSecondItems) => new
                    {
                        FirstItem = firstItem,
                        MatchingSecondItem = matchingSecondItems.FirstOrDefault()
                    })
                .Select(joinedItem =>
                {
                    if (joinedItem.MatchingSecondItem != null)
                    {
                        joinedItem.FirstItem.Navi = joinedItem.MatchingSecondItem;
                    }
                    return joinedItem.FirstItem;
                })
                .ToList();

            _basicProfile = profileResult;
            _naviProfile = naviResult;
            _favouriteMs = new ObservableCollection<FavouriteMs>(favouriteResult);
            _mobileSuitsSkillGroups = new ObservableCollection<MobileSuitWithSkillGroup>(msWithAltCostumes);
            _naviObservableCollection = new ObservableCollection<NaviWithNavigatorGroup>(naviWithAltCostumes);

            cpuTriadPartner = cpuTriadPartnerResult;
            SelectedTriadSkill1 = DataService.GetTriadSkill(cpuTriadPartner.Skill1);
            SelectedTriadSkill2 = DataService.GetTriadSkill(cpuTriadPartner.Skill2);
            SelectedTriadTeamBanner = DataService.GetTriadTeamBanner(cpuTriadPartner.TriadBackgroundPartsId);
            CustomizeComment = customizeCommentResult;

            _gamepadConfig = gamepadConfig;

            _customMessageGroupSetting = customMessageGroupSetting;

            _tagTeams = tagTeamResults;
        }
    }
    
    private void AddFavouriteMobileSuitItem()
    {
        if (_favouriteMs.Count >= maximumFavouriteMs)
        {
            Snackbar.Add($"{localizer["validate_addrow_1"]}{maximumFavouriteMs}{localizer["validate_addrow_2"]}", Severity.Warning);
            return;
        }

        var newTitle = new Title
            {
                TextId = 1,
                OrnamentId = 1,
                EffectId = 1,
                BackgroundPartsId = 1
            };

        var newItem = new FavouriteMs
            {
                MsId = 1,
                GaugeDesignId = 1,
                BgmPlayingMethod = BgmPlayingMethod.None,
                BgmList = new uint[] { },
                BattleNaviId = 1,
                BurstType = BurstType.Covering,
                DefaultTitle = newTitle,
                TriadTitle = newTitle,
                RankingTitle = newTitle
            };

        _favouriteMs.Add(newItem);
    }

    private void RemoveFavouriteUnit(CellContext<FavouriteMs> cellContext)
    {
        var itemHashCode = cellContext.Item.GetHashCode();
        var item = _favouriteMs.FirstOrDefault(x => x.GetHashCode() == itemHashCode);

        if (item != null)
            _favouriteMs.Remove(item);
    }
    
    private async Task OpenNaviChangeUiDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { _naviProfile.DefaultUiNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>(localizer["dialogtitle_navimenu"], parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _naviProfile.DefaultUiNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenNaviChangeBattleDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { _naviProfile.DefaultBattleNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>(localizer["dialogtitle_navibattle"], parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _naviProfile.DefaultBattleNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }
    
    private async Task OpenCpuTriadMobileSuitUiDialog()
    {
        var parameters = new DialogParameters { { "Data", cpuTriadPartner.MobileSuitId } };
        var dialog = await DialogService.ShowAsync<ChangeMobileSuitDialog>(localizer["dialogtitle_triadms"], parameters, OPTIONS);
        var result = await dialog.Result;
        
        if (!result.Canceled && result.Data != null)
        {
            cpuTriadPartner.MobileSuitId = (uint)result.Data;
            StateHasChanged();
        }
    }

    private async Task OpenCustomizeFavouriteMsDialog(FavouriteMs item)
    {
        var index = _favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters
        {
            { "Data", item },
            { "EnableImagePreview", EnableImagePreview }
        };
        var dialog = await DialogService.ShowAsync<CustomizeFavMsDialog>(localizer["dialogtitle_favms"], parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _favouriteMs[index] = (result.Data as FavouriteMs);
            StateHasChanged();
        }
    }

    private async Task SaveAll()
    {
        SaveAllButtonDisabled = true;
        HideSaveAllProgress = "visible";
        StateHasChanged();

        await SaveBasicProfile();
        await SaveNavigatorProfile();
        await SaveNaviCostume();
        await SaveFavouriteMobileSuits();
        await SaveMobileSuitsCostume();
        await SaveTriadCpuPartner();
        await SaveCustomizeComment();
        await SaveGamepadConfig();
        await SaveCommunicationMessageConfig();
        await SaveTeamTags();

        HideSaveAllProgress = "invisible";
        SaveAllButtonDisabled = false;
        StateHasChanged();
    }

    private async Task SaveBasicProfile()
    {
        if (_nameValidator.ValidatePlayerName(_basicProfile.UserName) is not null)
        {
            ShowBasicResponseSnack(new BasicResponse { Success = false }, localizer["save_hint_cardinfo"]);
            return;
        }

        HideProfileProgress = "visible";
        StateHasChanged();
        
        var dto = new UpdateBasicProfileRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            BasicProfile = _basicProfile
        };

        var response = await Http.PostAsJsonAsync("/card/updateBasicProfile", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_cardinfo"]);

        HideProfileProgress = "invisible";
        StateHasChanged();
    }

    private async Task SaveNavigatorProfile()
    {
        HideNaviProgress = "visible";
        StateHasChanged();

        var dto = new UpsertDefaultNaviRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            DefaultBattleNaviId = _naviProfile.DefaultBattleNaviId,
            DefaultUiNaviId = _naviProfile.DefaultUiNaviId,
        };

        var response = await Http.PostAsJsonAsync("/card/upsertDefaultNavi", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_navinfo"]);

        HideNaviProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveNaviCostume()
    {
        HideNaviCostumeProgress = "visible";
        StateHasChanged();

        var navis = _naviObservableCollection.Where(x => x.Navi != null).Select(x => x.Navi).ToList();

        var dto = new UpdateAllNaviCostumeRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            Navis = navis
        };

        var response = await Http.PostAsJsonAsync("/card/updateAllNaviCostume", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_navcostume"]);

        HideNaviCostumeProgress = "invisible";
        StateHasChanged();
    }

    private async Task SaveFavouriteMobileSuits()
    {
        HideFavMsProgress = "visible";
        StateHasChanged();

        var dto = new UpdateAllFavouriteMsRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            FavouriteMsList = _favouriteMs.ToList()
        };

        var response = await Http.PostAsJsonAsync("/card/updateAllFavouriteMs", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_favms"]);

        HideFavMsProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveMobileSuitsCostume()
    {
        HideMsCostumeProgress = "visible";
        StateHasChanged();

        var newSkillGroup = _mobileSuitsSkillGroups.Where(x => x.SkillGroup != null).Select(x => x.SkillGroup).ToList();

        var dto = new UpdateAllMsCostumeRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            MsSkillGroup = newSkillGroup
        };

        var response = await Http.PostAsJsonAsync("/card/updateAllMsCostume", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_mscostume"]);

        HideMsCostumeProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveTriadCpuPartner()
    {
        if (_nameValidator.ValidateTeamName(cpuTriadPartner.TriadTeamName) is not null)
        {
            ShowBasicResponseSnack(new BasicResponse { Success = false }, localizer["save_hint_triadcpupartner"]);
            return;
        }

        int totalLevel = cpuTriadPartner.ArmorLevel + cpuTriadPartner.ShootAttackLevel + cpuTriadPartner.InfightAttackLevel
                         + cpuTriadPartner.BoosterLevel + cpuTriadPartner.ExGaugeLevel + cpuTriadPartner.AiLevel;

        if (totalLevel > 500)
        {
            Snackbar.Add(localizer["save_hint_cpulimit"], Severity.Warning);
            return;
        }
        
        HideTriadCpuPartnerProgress = "visible";
        StateHasChanged();

        cpuTriadPartner.Skill1 = SelectedTriadSkill1?.Id ?? 0;
        cpuTriadPartner.Skill2 = SelectedTriadSkill2?.Id ?? 0;
        cpuTriadPartner.TriadBackgroundPartsId = SelectedTriadTeamBanner?.Id ?? 0;

        var dto = new UpdateCpuTriadPartnerRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            CpuTriadPartner = cpuTriadPartner
        };

        var response = await Http.PostAsJsonAsync("/card/updateCpuTriadPartner", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_triadcpupartner"]);

        HideTriadCpuPartnerProgress = "invisible";
        StateHasChanged();
    }

    private async Task SaveCustomizeComment()
    {
        HideCustomizeCommentProgress = "visible";
        StateHasChanged();
    
        if (CustomizeComment is null)
        {
            Snackbar.Add("Data Error for Customize Comment", Severity.Error);
            return;
        }
    
        var dto = new UpdateCustomizeCommentRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            CustomizeComment = CustomizeComment
        };
    
        var response = await Http.PostAsJsonAsync("/card/updateCustomizeComment", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
    
        ShowBasicResponseSnack(result, localizer["save_hint_customizecomment"]);
    
        HideCustomizeCommentProgress = "invisible";
        StateHasChanged();
    }

    private async Task SaveGamepadConfig()
    {
        HideGamepadConfigProgress = "visible";
        StateHasChanged();
        
        var dto = new UpsertGamepadConfigRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            GamepadConfig = _gamepadConfig
        };

        var response = await Http.PostAsJsonAsync("/card/upsertGamepadConfig", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_gamepadconfig"]);

        HideGamepadConfigProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveCommunicationMessageConfig()
    {
        var allMessageValid = AllMessageValid(_customMessageGroupSetting.StartGroup)
            && AllMessageValid(_customMessageGroupSetting.InBattleGroup)
            && AllMessageValid(_customMessageGroupSetting.ResultGroup);

        if (!allMessageValid)
        {
            ShowBasicResponseSnack(new BasicResponse { Success = false }, localizer["save_hint_commconfig"]);
            return;
        }

        HideCommunicationMessageProgress = "visible";
        StateHasChanged();
        
        var dto = new UpsertCustomMessagesRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            MessageSetting = _customMessageGroupSetting
        };
        
        var response = await Http.PostAsJsonAsync("/card/upsertCustomMessages", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_commconfig"]);

        HideCommunicationMessageProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveTeamTags()
    {
        var errorCount = 0;
        
        _tagTeams.ForEach(team =>
        {
            if (_nameValidator.ValidatePvPTeamName(team.Name) is not null)
            {
                errorCount++;
            }
        });
        
        if (errorCount > 0)
        {
            ShowBasicResponseSnack(new BasicResponse { Success = false }, localizer["save_hint_team"]);
            return;
        }

        HideTeamTagsProgress = "visible";
        StateHasChanged();
        
        var dto = new UpsertTeamsRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            Teams = _tagTeams
        };

        var response = await Http.PostAsJsonAsync("/card/upsertTeams", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, localizer["save_hint_team"]);

        HideTeamTagsProgress = "invisible";
        StateHasChanged();
    }

    private bool AllMessageValid(CustomMessageGroup customMessageGroup)
    {
        return _nameValidator.ValidateCustomizeMessage(customMessageGroup.UpMessage.MessageText) is null
            && _nameValidator.ValidateCustomizeMessage(customMessageGroup.DownMessage.MessageText) is null
            && _nameValidator.ValidateCustomizeMessage(customMessageGroup.LeftMessage.MessageText) is null
            && _nameValidator.ValidateCustomizeMessage(customMessageGroup.RightMessage.MessageText) is null;
    }

    public void ShowBasicResponseSnack(BasicResponse result, string context = "")
    {
        if (result.Success)
            Snackbar.Add($"{localizer["save_hint_update"]}{context}{localizer["save_hint_successful"]}", Severity.Success);
        else
            Snackbar.Add($"{localizer["save_hint_update"]}{context}{localizer["save_hint_failed"]}", Severity.Error);
    }
    
    // quick filter - filter gobally across multiple columns with the same input
    private Func<MobileSuitWithSkillGroup, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_msCostumeSearchString))
            return true;

        if (x.MobileSuit.Value.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.ValueJP.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.ValueCN.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        
        if (x.MobileSuit.Pilot.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.PilotJP.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.PilotCN.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        
        if (x.MobileSuit.Series.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.SeriesJP.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.SeriesCN.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };
    
    private Func<NaviWithNavigatorGroup, bool> _naviQuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_naviCostumeSearchString))
            return true;

        if (x.Navigator.Value.Contains(_naviCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Navigator.ValueJP.Contains(_naviCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Navigator.ValueCN.Contains(_naviCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };
    
    private async Task OnCostumeSelectChanged(IEnumerable<uint> selectedIds, MobileSuitWithSkillGroup context)
    {
        if (context.SkillGroup == null)
        {
            context.SkillGroup = new MsSkillGroup()
            {
                MstMobileSuitId = context.MobileSuit.Id,
                MsUsedNum = 0,
                CostumeId = selectedIds.FirstOrDefault(),
                TriadBuddyPoint = 0
            };
        }
        else
        {
            context.SkillGroup.CostumeId = selectedIds.FirstOrDefault();
        }
    }
    
    private async Task OnNaviCostumeSelectChanged(IEnumerable<uint> selectedIds, NaviWithNavigatorGroup context)
    {
        if (context.Navi == null)
        {
            context.Navi = new Navi()
            {
                Id = context.Navigator.Id,
                CostumeId = selectedIds.FirstOrDefault(),
                Familiarity = 0
            };
        }
        else
        {
            context.Navi.CostumeId = selectedIds.FirstOrDefault();
        }
    }
    
    public class MobileSuitWithSkillGroup
    {
        public MobileSuit MobileSuit { get; set; }

        public MsSkillGroup? SkillGroup { get; set; }
    }
    
    public class NaviWithNavigatorGroup
    {
        public Navigator Navigator { get; set; }
        public Navi? Navi { get; set; }
    }
}