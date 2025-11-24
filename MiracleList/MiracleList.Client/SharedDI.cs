using ITVisions.Blazor;
using MiracleListAPI;

namespace MiracleList.Client;

/// <summary>
/// Klassen per DI, die sowohl im Server als auch im Client benötigt werden
/// </summary>
public class SharedDI
{
 // eigene Dienste
 public static void AddServices(IServiceCollection services)
 {
  // Hilfsfunktionen
  services.AddBlazorUtil();

  // Für WebAPI-Backend
  services.AddSingleton(new HttpClient());
  services.AddScoped<MiracleListProxy>(); // oder services.AddScoped(typeof(MiracleListProxy));
 }
}
