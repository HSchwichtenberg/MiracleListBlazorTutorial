using ITVisions.Blazor;
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
        services.AddScoped<MiracleListProxy>(); // oder services.AddScoped(typeof(MiracleListProxy));
    }
}
