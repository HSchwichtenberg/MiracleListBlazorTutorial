using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiracleList.Client;
using MiracleList.Components;

var builder = WebApplication.CreateBuilder(args);

// DI der Kern-Dienste für Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// DI der eigenen Dienste, die sowohl im Server als auch im Client benötigt werden
SharedDI.AddServices(builder.Services);

var app = builder.Build();

//---------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
 app.UseWebAssemblyDebugging();
}
else
{
 app.UseExceptionHandler("/Error", createScopeForErrors: true);
 // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
 app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true); // Neu in .NET 10.0
app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MiracleList.Client._Imports).Assembly);

app.Run();