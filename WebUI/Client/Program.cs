using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using WebUI.Client;
using WebUI.Client.Extensions;
using WebUI.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddSingleton<IDataService, DataService>();

builder.Services.AddLocalization();
builder.Services.AddTransient<MudLocalizer, ResXMudLocalizer>();

var host = builder.Build();

var service = host.Services.GetRequiredService<IDataService>();
await service.InitializeAsync();

await host.SetDefaultCulture();

await host.RunAsync();