using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using Throw;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Response;

namespace WebUI.Client.Pages;

public partial class ViewCard
{
    [Parameter]
    public string ChipId { get; set; } = string.Empty;
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;
    
    private BasicProfile _basicProfile = null!;
    private List<UploadedImage> _uploadedImages = new();
    
    private string? errorMessage = null;
    
    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new BreadcrumbItem("Cards", href: "/Cards"),
    };
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem($"Card: {ChipId}", href: null, disabled: true));
        breadcrumbs.Add(new BreadcrumbItem(localizer["cardviewdetail"], href: $"/Cards/ViewDetail/{AccessCode}/{ChipId}", disabled: false));

        var profileResult = await Http.GetFromJsonAsync<BasicProfile>($"/card/getBasicDisplayProfile/{AccessCode}/{ChipId}");
        profileResult.ThrowIfNull();
        
        var uploadedImageResult = await Http.GetFromJsonAsync<List<UploadedImage>>($"/card/getUploadedImages/{AccessCode}/{ChipId}");
        uploadedImageResult.ThrowIfNull();
        
        _basicProfile = profileResult;
        _uploadedImages = uploadedImageResult;
    }
}