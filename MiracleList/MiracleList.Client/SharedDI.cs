using ITVisions.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using MiracleListAPI;

namespace MiracleList.Client;

public class SharedDI
{
 // eigene Dienste
 public static void AddServices(IServiceCollection services)
 {
  // Hilfsfunktionen
  services.AddBlazorUtil();

  // Für WebAPI-Backend
  services.AddSingleton(new HttpClient());
  services.AddScoped<MiracleListProxy>();

  // weitere Services
  //services.AddScoped<AuthenticationManager>();

  #region DI Authentifizierungsdienste
  services.AddOptions(); // notwendig für AuthenticationStateProvider
  services.AddCascadingAuthenticationState(); // neu seit Blazor 8.0
  services.AddScoped<AuthenticationStateProvider, AuthenticationManager>();
  services.AddAuthorizationCore();
  #endregion

 }
}
