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

namespace WebUI.Client.Pages;

public partial class CustomizeCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;

    // private MudDataGrid<FavouriteMs> favMsDataGrid;

    private BasicProfile                       basicProfile = null!;
    private NaviProfile                        naviProfile  = null!;
    private ObservableCollection<FavouriteMs> favouriteMs  = new();

    private string? errorMessage = null;

    private readonly int maximumFavouriteMs = 6;

    private bool SwitchOpenRecord { get; set; }
    private bool SwitchOpenEchelon { get; set; }

    private string HideSaveAllProgress { get; set; } = "invisible";
    private string HideProfileProgress { get; set; } = "invisible";
    private string HideNaviProgress { get; set; } = "invisible";
    private string HideFavMsProgress { get; set; } = "invisible";
    private string HideMsCostumeProgress { get; set; } = "invisible";
    private const int PLAYER_NAME_MAX_LENGTH = 12;

    private MobileSuit? SelectedHasCostumeMsValue { get; set; }
    private Costume? SelectedMsCostumeValue { get; set; }

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

        //var json = System.Text.Json.JsonSerializer.Serialize(naviResult);
        //Logger.LogInformation($"{json}");

        basicProfile = profileResult;
        naviProfile = naviResult;
        favouriteMs = new ObservableCollection<FavouriteMs>(favouriteResult);

        SwitchOpenRecord = Convert.ToBoolean(basicProfile.OpenRecord);
        SwitchOpenEchelon = Convert.ToBoolean(basicProfile.OpenEchelon);
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
        if (favouriteMs.Count > maximumFavouriteMs)
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

        favouriteMs.Add(newItem);
    }

    private void RemoveFavouriteUnit(CellContext<FavouriteMs> cellContext)
    {
        var itemHashCode = cellContext.Item.GetHashCode();
        var item = favouriteMs.FirstOrDefault(x => x.GetHashCode() == itemHashCode);

        if (item != null)
            favouriteMs.Remove(item);
    }

    private async Task OpenProfileChangeBgmOrderDialog()
    {
        var parameters = new DialogParameters { { "Data", basicProfile.DefaultBgmList } };
        var dialog = await DialogService.ShowAsync<ChangeBgmOrderDialog>("Add / Change Bgm Order", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            basicProfile.DefaultBgmList = (result.Data as uint[])!;
            StateHasChanged();
        }
    }

    private async Task OpenProfileChangeGaugeDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { basicProfile.DefaultGaugeDesignId } } };
        var dialog = await DialogService.ShowAsync<ChangeGaugeDialog>("Change Gauge UI", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            basicProfile.DefaultGaugeDesignId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenNaviChangeUiDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { naviProfile.DefaultUiNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>("Change UI navigator", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            naviProfile.DefaultUiNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenNaviChangeBattleDialog()
    {
        var parameters = new DialogParameters { { "Data", new[] { naviProfile.DefaultBattleNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>("Change in battle navigator", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            naviProfile.DefaultBattleNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenChangeFavouriteMsDialog(FavouriteMs item)
    {
        var index = favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters { { "Data", item.MsId } };
        var dialog = await DialogService.ShowAsync<ChangeMobileSuitDialog>("Change favourite mobile suit", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            favouriteMs[index].MsId = (uint)result.Data;
            StateHasChanged();
        }
    }

    private async Task OpenFavMsChangeBgmOrderDialog(FavouriteMs item)
    {
        var index = favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters { { "Data", item.BgmList } };
        var dialog = await DialogService.ShowAsync<ChangeBgmOrderDialog>("Add / Change bgm order", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            favouriteMs[index].BgmList = (result.Data as uint[])!;
            StateHasChanged();
        }
    }

    private async Task OpenFavMsChangeNaviDialog(FavouriteMs item)
    {
        var index = favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters { { "Data", new[] { item.BattleNaviId } } };
        var dialog = await DialogService.ShowAsync<ChangeNavigatorDialog>("Change in battle navigator", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            favouriteMs[index].BattleNaviId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task OpenFavMsChangeGaugeDialog(FavouriteMs item)
    {
        var index = favouriteMs.IndexOf(item);

        if (index == -1)
            throw new ArgumentException("Selected item is not part of the provided items list.");

        var parameters = new DialogParameters {{ "Data", new[] { item.GaugeDesignId }}};
        var dialog = await DialogService.ShowAsync<ChangeGaugeDialog>("Change gauge UI", parameters, OPTIONS);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data != null)
        {
            favouriteMs[index].GaugeDesignId = (result.Data as uint[])!.FirstOrDefault();
            StateHasChanged();
        }
    }

    private async Task SaveAll()
    {
        HideSaveAllProgress = "visible";
        StateHasChanged();

        await SaveBasicProfile();
        await SaveNavigatorProfile();
        await SaveFavouriteMobileSuits();

        HideSaveAllProgress = "invisible";
        StateHasChanged();
    }

    private async Task SaveBasicProfile()
    {
        HideProfileProgress = "visible";
        StateHasChanged();

        basicProfile.OpenEchelon = Convert.ToUInt32(SwitchOpenEchelon);
        basicProfile.OpenRecord = Convert.ToUInt32(SwitchOpenRecord);

        var dto = new UpdateBasicProfileRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            BasicProfile = basicProfile
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
            DefaultBattleNaviId = naviProfile.DefaultBattleNaviId,
            DefaultUiNaviId = naviProfile.DefaultUiNaviId,
        };

        var response = await Http.PostAsJsonAsync("/card/upsertDefaultNavi", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, "navigatior infos");

        HideNaviProgress = "invisible";
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
            FavouriteMsList = favouriteMs.ToList()
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
        if (SelectedHasCostumeMsValue is null)
        {
            Snackbar.Add("Please select MS for Costume Part", Severity.Warning);
            return;
        }

        if (SelectedMsCostumeValue is null)
        {
            Snackbar.Add("Please select Costume for Costume Part", Severity.Warning);
            return;
        }

        HideMsCostumeProgress = "visible";
        StateHasChanged();

        var dto = new UpsertMsCostumeRequest()
        {
            AccessCode = AccessCode,
            ChipId = ChipId,
            MobileSuit = new BaseMobileSuit
            {
                MobileSuitId = SelectedHasCostumeMsValue.Id,
                CostumeId = SelectedMsCostumeValue.Id
            }
        };

        var response = await Http.PostAsJsonAsync("/card/upsertMsCostume", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();

        ShowBasicResponseSnack(result, "mobile suit costume");

        HideMsCostumeProgress = "invisible";
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
        const string pattern = @"^[a-zA-Z0-9!?,./\-+:<>_\\@*#&=() ]{1,12}$";

        return playerName.Length switch
        {
            0 => "Player name cannot be empty!",
            > PLAYER_NAME_MAX_LENGTH => "Player name cannot be longer than 12 characters!",
            _ => !Regex.IsMatch(playerName, pattern) ? "Player name contains invalid character!" : null
        };
    }
}