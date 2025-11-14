using System.Net;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiracleList.Client;
using MiracleList.Components;
using Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

#region Authentifizierung und Autorisierung nutzen
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddScheme<MLAuthSchemeOptions, MLAuthSchemeHandler>("ML", opts => { }); // notwendig, damit bei Static SSR Prerendering Zugriffe auf Unterseiten zum Fehler 401 führen, der dann auf /login umgeleitet wird
#endregion

SharedDI.AddServices(builder.Services);

var app = builder.Build();

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

#region Authentifizierung und Autorisierung nutzen
// Bei Static SSR: Umleiten von 401-Fehler auf die /Login-Seite
app.UseStatusCodePages(async context =>
{
 var request = context.HttpContext.Request;
 var response = context.HttpContext.Response;
 if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
 {
  response.Redirect("/Login");  //redirect to the login page.
 }
});

app.UseAuthentication();
app.UseAuthorization();
#endregion

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true); // Neu in .NET 10.0
app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MiracleList.Client._Imports).Assembly);

app.Run();