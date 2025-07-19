using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using WebUIOver.Client;
using WebUIOver.Client.Command.CustomizeCard.Fill;
using WebUIOver.Client.Command.CustomizeCard.Save;
using WebUIOver.Client.Command.ViewCard.Filler;
using WebUIOver.Client.Extensions;
using WebUIOver.Client.Services;
using WebUIOver.Client.Services.Common;
using WebUIOver.Client.Services.CustomizeComment;
using WebUIOver.Client.Services.CustomMessage;
using WebUIOver.Client.Services.Display;
using WebUIOver.Client.Services.Gamepad;
using WebUIOver.Client.Services.Mastery;
using WebUIOver.Client.Services.MS;
using WebUIOver.Client.Services.Name;
using WebUIOver.Client.Services.Navi;
using WebUIOver.Client.Services.Preview;
using WebUIOver.Client.Services.Selector;
using WebUIOver.Client.Services.Stamp;
using WebUIOver.Client.Services.Sticker;
using WebUIOver.Client.Services.Team;
using WebUIOver.Client.Services.Title;
using WebUIOver.Client.Services.Triad;
using WebUIOver.Client.Validator;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services
    .AddBlazorise(options => options.Immediate = true)
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddSingleton<INameService, NameService>();
builder.Services.AddSingleton<IGeneralPreviewService, GeneralPreviewService>();

builder.Services.AddSingleton<IResponseSnackService, ResponseSnackService>();

builder.Services.AddSingleton<ICommonDataService, CommonDataService>();
builder.Services.AddSingleton<IFamiliarityDataService, FamiliarityDataService>();
builder.Services.AddSingleton<IDisplayOptionDataService, DisplayOptionDataService>();
builder.Services.AddSingleton<ITitleDataService, TitleDataService>();
builder.Services.AddSingleton<INaviDataService, NaviDataService>();
builder.Services.AddSingleton<IMobileSuitDataService, MobileSuitDataService>();
builder.Services.AddSingleton<IStampDataService, StampDataService>();
builder.Services.AddSingleton<ICustomMessageTemplateService, CustomMessageTemplateService>();
builder.Services.AddSingleton<ICustomizeCommentService, CustomizeCommentService>();
builder.Services.AddSingleton<IStickerService, StickerService>();
builder.Services.AddSingleton<ITriadDataService, TriadDataService>();
builder.Services.AddSingleton<ITeamDataService, TeamDataService>();
builder.Services.AddSingleton<IGamepadDataService, GamepadDataService>();
builder.Services.AddSingleton<ITriadStageDataService, TriadStageDataService>();

builder.Services.AddSingleton<INameValidator, NameValidator>();

builder.Services.AddSingleton<BasicProfileFiller, BasicProfileFiller>();
builder.Services.AddSingleton<PlayerLevelProfileFiller, PlayerLevelProfileFiller>();
builder.Services.AddSingleton<NaviProfileFiller, NaviProfileFiller>();
builder.Services.AddSingleton<MsSkillGroupAggregator, MsSkillGroupAggregator>();
builder.Services.AddSingleton<FavouriteMobileSuitFiller, FavouriteMobileSuitFiller>();
builder.Services.AddSingleton<MsAlternativeCostumeAggregator, MsAlternativeCostumeAggregator>();
builder.Services.AddSingleton<MsAlternativeSkinAggregator, MsAlternativeSkinAggregator>();
builder.Services.AddSingleton<CustomMessageGroupFiller, CustomMessageGroupFiller>();
builder.Services.AddSingleton<DefaultStickerSettingFiller, DefaultStickerSettingFiller>();
builder.Services.AddSingleton<MobileSuitStickersSettingFiller, MobileSuitStickersSettingFiller>();
builder.Services.AddSingleton<CpuTriadPartnerFiller, CpuTriadPartnerFiller>();
builder.Services.AddSingleton<TeamResponseFiller, TeamResponseFiller>();
builder.Services.AddSingleton<GamepadConfigFiller, GamepadConfigFiller>();
builder.Services.AddSingleton<TrainingProfileFiller, TrainingProfileFiller>();

builder.Services.AddSingleton<BasicProfileSaver, BasicProfileSaver>();
builder.Services.AddSingleton<NaviProfileSaver, NaviProfileSaver>();
builder.Services.AddSingleton<NaviCostumeSaver, NaviCostumeSaver>();
builder.Services.AddSingleton<FavouriteMobileSuitSaver, FavouriteMobileSuitSaver>();
builder.Services.AddSingleton<MsCostumeSaver, MsCostumeSaver>();
builder.Services.AddSingleton<MsSkinSaver, MsSkinSaver>();
builder.Services.AddSingleton<CustomMessageGroupSaver, CustomMessageGroupSaver>();
builder.Services.AddSingleton<DefaultStickerSettingSaver, DefaultStickerSettingSaver>();
builder.Services.AddSingleton<MobileSuitStickerSettingsSaver, MobileSuitStickerSettingsSaver>();
builder.Services.AddSingleton<CpuTriadPartnerSaver, CpuTriadPartnerSaver>();
builder.Services.AddSingleton<TeamSaver, TeamSaver>();
builder.Services.AddSingleton<GamepadConfigSaver, GamepadConfigSaver>();
builder.Services.AddSingleton<TrainingProfileSaver, TrainingProfileSaver>();

builder.Services.AddSingleton<TriadCourseOverallResultFiller, TriadCourseOverallResultFiller>();
builder.Services.AddSingleton<BattleHistoriesFiller, BattleHistoriesFiller>();

builder.Services.AddSingleton<ISelectorService, SelectorService>();

builder.Services.AddLocalization();
builder.Services.AddTransient<MudLocalizer, ResXMudLocalizer>();

var host = builder.Build();

var commonDataService = host.Services.GetRequiredService<ICommonDataService>();
await commonDataService.InitializeAsync();

var familiarityDataService = host.Services.GetRequiredService<IFamiliarityDataService>();
await familiarityDataService.InitializeAsync();

var displayOptionDataService = host.Services.GetRequiredService<IDisplayOptionDataService>();
await displayOptionDataService.InitializeAsync();

var titleDataService = host.Services.GetRequiredService<ITitleDataService>();
await titleDataService.InitializeAsync();

var naviDataService = host.Services.GetRequiredService<INaviDataService>();
await naviDataService.InitializeAsync();

var mobileSuitDataService = host.Services.GetRequiredService<IMobileSuitDataService>();
await mobileSuitDataService.InitializeAsync();

var stampDataService = host.Services.GetRequiredService<IStampDataService>();
await stampDataService.InitializeAsync();

var customMessageTemplateService = host.Services.GetRequiredService<ICustomMessageTemplateService>();
await customMessageTemplateService.InitializeAsync();

var customizeCommentService = host.Services.GetRequiredService<ICustomizeCommentService>();
await customizeCommentService.InitializeAsync();

var stickerService = host.Services.GetRequiredService<IStickerService>();
await stickerService.InitializeAsync();

var triadDataService = host.Services.GetRequiredService<ITriadDataService>();
await triadDataService.InitializeAsync();

var teamDataService = host.Services.GetRequiredService<ITeamDataService>();
await teamDataService.InitializeAsync();

var gamepadDataService = host.Services.GetRequiredService<IGamepadDataService>();
await gamepadDataService.InitializeAsync();

var triadStageDataService = host.Services.GetRequiredService<ITriadStageDataService>();
await triadStageDataService.InitializeAsync();

await host.SetDefaultCulture();

await host.RunAsync();