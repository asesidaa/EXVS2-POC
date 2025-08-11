﻿using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

namespace WebUIOver.Client.Extensions
{
	public static class WebAssemblyHostExtension
	{
		public async static Task SetDefaultCulture(this WebAssemblyHost host)
		{
			var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
			var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
			CultureInfo culture;
			if (result != null)
				culture = new CultureInfo(result);
			else
				culture = new CultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
		}
	}
}
