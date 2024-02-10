using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;
using Throw;
using WebUIVanilla.Shared.Dto.Common;
using WebUIVanilla.Shared.Dto.Response;

namespace WebUIVanilla.Client.Pages;

public partial class ViewCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }

    private BasicProfile _basicProfile = null!;
    private TriadCourseOverallResult _triadCourseOverallResult = null!;
    
    private string? errorMessage = null;
    
    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new BreadcrumbItem("Cards", href: "/Cards"),
    };
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem($"Card: {ChipId}", href: null, disabled: true));
        breadcrumbs.Add(new BreadcrumbItem(localizer["cardviewdetail"], href: $"/Cards/ViewDetail/{ChipId}", disabled: false));

        AccessCode = await _jsRuntime.InvokeAsync<string>("accessCode.get");

        var profileResult = await Http.GetFromJsonAsync<BasicProfile>($"/card/getBasicDisplayProfile/{AccessCode}/{ChipId}");
        profileResult.ThrowIfNull();
        
        var triadCourseOverallResult = await Http.GetFromJsonAsync<TriadCourseOverallResult>($"/card/getTriadCourseOverallResult/{AccessCode}/{ChipId}");
        triadCourseOverallResult.ThrowIfNull();
        
        _basicProfile = profileResult;
        _triadCourseOverallResult = triadCourseOverallResult;
    }
}