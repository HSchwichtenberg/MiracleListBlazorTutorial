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
<<<<<<< HEAD
  services.AddScoped<AuthenticationStateProvider, AuthenticationManager>(); // unser AuthenticationManager übernimmt die Rolle des AuthenticationStateProvider
=======
  services.AddScoped<AuthenticationStateProvider, AuthenticationManager>();
>>>>>>> 62da502e57cfb015f9f6f58f8f7ae21ec2712220
  services.AddAuthorizationCore();
  #endregion

 }
}
