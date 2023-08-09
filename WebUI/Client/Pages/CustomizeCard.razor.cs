using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Throw;
using WebUI.Client.Pages.Dialogs;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Dto.Json;
using WebUI.Shared.Dto.Request;
using WebUI.Shared.Dto.Response;
using static MudBlazor.CategoryTypes;

namespace WebUI.Client.Pages;

public partial class CustomizeCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;

    // private MudDataGrid<FavouriteMs> favMsDataGrid;

    private BasicProfile _basicProfile = null!;
    private NaviProfile _naviProfile  = null!;
    private ObservableCollection<FavouriteMs> _favouriteMs  = new();
    private ObservableCollection<MobileSuitWithSkillGroup> _mobileSuitsSkillGroups = new();
    private ObservableCollection<NaviWithNavigatorGroup> _naviObservableCollection = new();
    private CpuTriadPartner cpuTriadPartner = null;
    private GamepadConfig _gamepadConfig = null;
    private CustomMessageGroupSetting _customMessageGroupSetting = null!;
    

    private string? errorMessage = null;

    private readonly int maximumFavouriteMs = 6;
    
    private string HideSaveAllProgress { get; set; } = "invisible";
    private string HideProfileProgress { get; set; } = "invisible";
    private string HideNaviProgress { get; set; } = "invisible";
    private string HideFavMsProgress { get; set; } = "invisible";
    private string HideMsCostumeProgress { get; set; } = "invisible";
    private string HideNaviCostumeProgress { get; set; } = "invisible";
    private string _msCostumeSearchString { get; set; }
    private string _naviCostumeSearchString { get; set; }

    private readonly int[] _pageSizeOptions = { 5, 10, 25, 50, 100 };

    private string HideTriadCpuPartnerProgress { get; set; } = "invisible";
    private string HideCustomizeCommentProgress { get; set; } = "invisible";
    private string HideGamepadConfigProgress { get; set; } = "invisible";
    private string HideCommunicationMessageProgress { get; set; } = "invisible";
    private const int PLAYER_NAME_MAX_LENGTH = 12;
    private const int MESSAGE_MAX_LENGTH = 10;

    private IdValuePair? SelectedTriadSkill1 { get; set; }
    private IdValuePair? SelectedTriadSkill2 { get; set; }
    private IdValuePair? SelectedTriadTeamBanner { get; set; }
    private CustomizeComment? CustomizeComment { get; set; }

    private bool CanCustomizeBeforeBattleUpTextMessage =>
        _customMessageGroupSetting?.StartGroup?.UpMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeBeforeBattleDownTextMessage =>
        _customMessageGroupSetting?.StartGroup?.DownMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeBeforeBattleLeftTextMessage =>
        _customMessageGroupSetting?.StartGroup?.LeftMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeBeforeBattleRightTextMessage =>
        _customMessageGroupSetting?.StartGroup?.RightMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeInBattleUpTextMessage =>
        _customMessageGroupSetting?.InBattleGroup?.UpMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeInBattleDownTextMessage =>
        _customMessageGroupSetting?.InBattleGroup?.DownMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeInBattleLeftTextMessage =>
        _customMessageGroupSetting?.InBattleGroup?.LeftMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeInBattleRightTextMessage =>
        _customMessageGroupSetting?.InBattleGroup?.RightMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeAfterBattleUpTextMessage =>
        _customMessageGroupSetting?.ResultGroup?.UpMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeAfterBattleDownTextMessage =>
        _customMessageGroupSetting?.ResultGroup?.DownMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeAfterBattleLeftTextMessage =>
        _customMessageGroupSetting?.ResultGroup?.LeftMessage?.UniqueMessageId == 0;
    
    private bool CanCustomizeAfterBattleRightTextMessage =>
        _customMessageGroupSetting?.ResultGroup?.RightMessage?.UniqueMessageId == 0;

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
        breadcrumbs.Add(new BreadcrumbItem($"Card: {ChipId}", href: null, disabled: true));
        breadcrumbs.Add(new BreadcrumbItem("Option", href: $"/Cards/Customize/{AccessCode}/{ChipId}", disabled: false));

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

        //var json = System.Text.Json.JsonSerializer.Serialize(naviResult);
        //Logger.LogInformation($"{json}");

        // assign costume selection id from ms skill group to the mobile suit list
        var mobileSuitList = DataService.GetMobileSuitSortedById().Select(x => new MobileSuitWithSkillGroup { MobileSuit = x }).ToList();
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
    }

    Func<BgmPlayingMethod, string> converter = p => p.ToString();

    private string GetMobileSuitName(uint id)
    {
        var navigator = DataService.GetMobileSuitById(id);

        return navigator?.NameEN ?? "Unknown Mobile Suit";
    }

    private string GetNaviName(uint id)
    {
        var navigator = DataService.GetNavigatorById(id);

        return navigator?.NameEN ?? "Unknown Navigator";
    }

    private string GetGaugeName(uint id)
    {
        var navigator = DataService.GetGaugeById(id);

        return navigator?.NameEN ?? "Unknown Gauge";
    }

    private void AddFavouriteMobileSuitItem()
    {
        if (_favouriteMs.Count > maximumFavouriteMs)
        {
            Snackbar.Add($"Cannot add more than {maximumFavouriteMs} entries!", Severity.Warning);
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

    private async Task OpenProfileChangeBgmOrderDialog()
    {
        var parameters = new DialogParameters { { "Data", _basicProfile.DefaultBgmList } };
        var dialog = await DialogService.ShowAsync<ChangeBgmOrderDialog>("Add / Change Bgm Order", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            _basicProfile.DefaultBgmList = (result.Data as uint[])!;
            StateHasChanged();
        }
    }

    private async Task OpenProfileChangeGaugeDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { _basicProfile.DefaultGaugeDesignId } } };
        var dialog = await DialogService.ShowAsync<ChangeGaugeDialog>("Change Gauge UI", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _basicProfile.DefaultGaugeDesignId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenNaviChangeUiDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { _naviProfile.DefaultUiNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>("Change UI navigator", parameters, OPTIONS);
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
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>("Change in battle navigator", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _naviProfile.DefaultBattleNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenChangeFavouriteMsDialog(FavouriteMs item)
    {
        var index = _favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters { { "Data", item.MsId } };
        var dialog = await DialogService.ShowAsync<ChangeMobileSuitDialog>("Change favourite mobile suit", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _favouriteMs[index].MsId = (uint)result.Data;
            StateHasChanged();
        }
    }

    private async Task OpenFavMsChangeBgmOrderDialog(FavouriteMs item)
    {
        var index = _favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters { { "Data", item.BgmList } };
        var dialog = await DialogService.ShowAsync<ChangeBgmOrderDialog>("Add / Change bgm order", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _favouriteMs[index].BgmList = (result.Data as uint[])!;
            StateHasChanged();
        }
    }

    private async Task OpenFavMsChangeNaviDialog(FavouriteMs item)
    {
        var index = _favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters { { "Data", new[] { item.BattleNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>("Change in battle navigator", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _favouriteMs[index].BattleNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenFavMsChangeGaugeDialog(FavouriteMs item)
    {
        var index = _favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters {{ "Data", new[] { item.GaugeDesignId }}};
        var dialog = await DialogService.ShowAsync<ChangeGaugeDialog>("Change gauge UI", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _favouriteMs[index].GaugeDesignId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }
    
    private async Task OpenCpuTriadMobileSuitUiDialog()
    {
        var parameters = new DialogParameters { { "Data", cpuTriadPartner.MobileSuitId } };
        var dialog = await DialogService.ShowAsync<ChangeMobileSuitDialog>("Change CPU Triad Partner MS", parameters, OPTIONS);
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

        var parameters = new DialogParameters { { "Data", item } };
        var dialog = await DialogService.ShowAsync<CustomizeFavMsDialog>("Customizing favourite mobile suit", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            _favouriteMs[index] = (result.Data as FavouriteMs);
            StateHasChanged();
        }
    }

    private async Task SaveAll()
    {
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

        HideSaveAllProgress = "invisible";
        StateHasChanged();
    }

    private async Task SaveBasicProfile()
    {
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

        ShowBasicResponseSnack(result, "card infos");

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

        ShowBasicResponseSnack(result, "navigatior infos");

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

        ShowBasicResponseSnack(result, "navi costume");

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

        ShowBasicResponseSnack(result, "favourite mobile suit infos");

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

        ShowBasicResponseSnack(result, "mobile suit costume");

        HideMsCostumeProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveTriadCpuPartner()
    {
        int totalLevel = cpuTriadPartner.ArmorLevel + cpuTriadPartner.ShootAttackLevel + cpuTriadPartner.InfightAttackLevel
                         + cpuTriadPartner.BoosterLevel + cpuTriadPartner.ExGaugeLevel + cpuTriadPartner.AiLevel;
        if (totalLevel > 500)
        {
            Snackbar.Add("Sum of CPU Level cannot exceed 500", Severity.Warning);
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

        ShowBasicResponseSnack(result, "Triad CPU Partner");

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

        ShowBasicResponseSnack(result, "Customize Comment");

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

        ShowBasicResponseSnack(result, "Gamepad Config");

        HideGamepadConfigProgress = "invisible";
        StateHasChanged();
    }
    
    private async Task SaveCommunicationMessageConfig()
    {
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

        ShowBasicResponseSnack(result, "Communication Message Config");

        HideCommunicationMessageProgress = "invisible";
        StateHasChanged();
    }

    private void ShowBasicResponseSnack(BasicResponse result, string context = "")
    {
        if (result.Success)
            Snackbar.Add($"Updatng {context} successful!", Severity.Success);
        else
            Snackbar.Add($"Updating {context} failed!", Severity.Error);
    }

    private static string? ValidatePlayerName(string playerName)
    {
        return ValidateName(playerName, "Player name");
    }
    
    private static string? ValidateTeamName(string teamName)
    {
        return ValidateName(teamName, "Triad CPU Team name");
    }
    
    private static string? ValidateCustomizeMessage(string message)
    {
        return ValidateMessage(message, "Message");
    }

    private static String? ValidateName(string name, string errorMessagePart)
    {
        const string pattern = @"^[ 一-龯ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９-_ー＜＞＋－＊÷＝；：←／＼＿｜・＠！？＆★（）＾◇∀Ξν×†ω♪♭#∞〆→↓↑％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]「」『』【】]{1,12}$";

        return name.Length switch
        {
            0 => errorMessagePart + " cannot be empty!",
            > PLAYER_NAME_MAX_LENGTH => errorMessagePart + " cannot be longer than 12 characters!",
            _ => !Regex.IsMatch(name, pattern) ? errorMessagePart + " contains invalid character!" : null
        };
    }
    
    private static String? ValidateMessage(string message, string errorMessagePart)
    {
        const string pattern = @"^[ 一-龯ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９ー＜＞＋－＊÷＝；：←／＼＿｜・＠！？＆★（）＾◇∀Ξν×†ω♪♭#∞〆→↓↑％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]「」『』【】]{1,12}$";

        return message.Length switch
        {
            0 => null,
            > MESSAGE_MAX_LENGTH => errorMessagePart + " cannot be longer than 10 characters!",
            _ => !Regex.IsMatch(message, pattern) ? errorMessagePart + " contains invalid character!" : null
        };
    }

    // quick filter - filter gobally across multiple columns with the same input
    private Func<MobileSuitWithSkillGroup, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_msCostumeSearchString))
            return true;

        if (x.MobileSuit.NameEN.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.NameJP.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.MobileSuit.NameCN.Contains(_msCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };
    
    private Func<NaviWithNavigatorGroup, bool> _naviQuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_naviCostumeSearchString))
            return true;

        if (x.Navigator.NameEN.Contains(_naviCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Navigator.NameJP.Contains(_naviCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Navigator.NameCN.Contains(_naviCostumeSearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    private async Task OnOpenRecordChanged(IEnumerable<uint> selectedIds, BasicProfile basicProfile)
    {
        basicProfile.OpenRecord = selectedIds.FirstOrDefault();
    }
    
    private async Task OnOpenEchelonChanged(IEnumerable<uint> selectedIds, BasicProfile basicProfile)
    {
        basicProfile.OpenEchelon = selectedIds.FirstOrDefault();
    }
    
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