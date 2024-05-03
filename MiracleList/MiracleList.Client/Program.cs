using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiracleList.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

SharedDI.AddServices(builder.Services);

await builder.Build().RunAsync();
