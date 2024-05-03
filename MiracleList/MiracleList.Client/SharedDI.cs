using ITVisions;
using ITVisions.Blazor;

namespace MiracleList.Client;

public class SharedDI
{
 // eigene Dienste
 public static void AddServices(IServiceCollection services)
 {
  // Für WebAPI-Backend
  services.AddSingleton(new HttpClient());
  services.AddScoped(typeof(MiracleListAPI.MiracleListProxy));

  // Hilfsfunktionen
  services.AddBlazorUtil();
 }
}
