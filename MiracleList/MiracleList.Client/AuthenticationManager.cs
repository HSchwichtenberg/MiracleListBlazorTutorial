using ITVisions.Blazor;
using MiracleListAPI;

namespace MiracleList.Client;

// Primärkonstruktorparameter werden per DI befüllt!!!
public class AuthenticationManager(MiracleListProxy proxy, BlazorUtil util)
{

 public string Token { get; set; }

 public async Task<bool> Login()
 {
  var loginInfo = new LoginInfo();
  loginInfo.ClientID = "TODO";
  loginInfo.Username = "Maxi@Mustermann.de";
  loginInfo.Password = "geheim";
  try
  {
   if (loginInfo.ClientID == "TODO")
   {
    throw new ApplicationException("Sie müssen im AuthenticationManager Ihre Client-ID eintragen, siehe http://miraclelistbackend.azurewebsites.net/!");
   }

   var loginInfoResult = await proxy.LoginAsync(loginInfo);

   if (loginInfoResult != null && !String.IsNullOrEmpty(loginInfoResult.Token)
    )
   {
    // erfolgreich
    util.Log("Anmeldung ok. Token = " + loginInfoResult.Token);
    this.Token = loginInfoResult.Token;
    return true;
   }
   else
   {
    // nicht erfolgreich
    util.Warn("Anmeldung nicht ok " + loginInfoResult?.Message);
    this.Token = "";
    return false;
   }
  }
  catch (Exception ex)
  {
   util.Warn("Anmeldung nicht möglich " + ex.Message);
   this.Token = "";
   return false;
  }

 }

}
