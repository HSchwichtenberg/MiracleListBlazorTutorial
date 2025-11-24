using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiracleList.Client;

// Setze die aktuelle Sprache auf Deutsch
System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// DI der eigenen Dienste, die sowohl im Server als auch im Client benötigt werden
SharedDI.AddServices(builder.Services);

await builder.Build().RunAsync();
