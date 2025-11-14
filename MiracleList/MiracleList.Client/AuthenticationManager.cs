using System.Security.Claims;
using ITVisions.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using MiracleListAPI;

namespace MiracleList.Client;

// Primärkonstruktorparameter werden per DI befüllt!!!
public class AuthenticationManager(MiracleListProxy proxy, BlazorUtil util) : AuthenticationStateProvider
{
 private const string clientID = "TODO"; // https://miraclelistbackend.azurewebsites.net/clientid

 public string Token { get; set; }

 public LoginInfo CurrentLoginInfo { get; set; }

 public async Task<bool> Login(string username, string password)
 {
  var loginInfo = new LoginInfo();
  loginInfo.ClientID = clientID;
  loginInfo.Username = username;
  loginInfo.Password = password;
  return await LoginInternal(loginInfo);
 }

 /// <summary>
 /// Nur für Debugging
 /// </summary>
 /// <returns></returns>
 public async Task<bool> Login()
 {
  var loginInfo = new LoginInfo();
  loginInfo.ClientID = clientID;
  loginInfo.Username = "Maxi@Mustermann.de";
  loginInfo.Password = "geheim";
  return await LoginInternal(loginInfo);

 }

 private async Task<bool> LoginInternal(LoginInfo loginInfo)
 {
  try
  {
   CurrentLoginInfo = await proxy.LoginAsync(loginInfo);

   if (CurrentLoginInfo != null && !String.IsNullOrEmpty(CurrentLoginInfo.Token)
    )
   {
    // erfolgreich
    util.Log("Anmeldung ok. Token = " + CurrentLoginInfo.Token);
    this.Token = CurrentLoginInfo.Token;
    Notify();
    return true;
   }
   else
   {
    // nicht erfolgreich
    util.Warn("Anmeldung nicht ok " + CurrentLoginInfo?.Message);
    this.Token = "";
    Notify();
    return false;
   }
  }
  catch (Exception ex)
  {
   util.Warn("Anmeldung nicht möglich " + ex.Message);
   this.Token = "";
   Notify();
   return false;
  }
 }

 private void Notify()
 {
  util.Log("Notify: " + CurrentLoginInfo?.Username);
  this.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
 }

 public override Task<AuthenticationState> GetAuthenticationStateAsync()
 {
  if (this.CurrentLoginInfo != null && !String.IsNullOrEmpty(this.CurrentLoginInfo.Token) && !String.IsNullOrEmpty(proxy.BaseUrl))
  {
   const string authType = "MiracleList WebAPI Authentication";
   var identity = new ClaimsIdentity(new[]
   {
    new Claim("Backend", proxy.BaseUrl),
    new Claim("ClaimTypes.Sid", this.CurrentLoginInfo.Token), // use SID claim for token
    new Claim(ClaimTypes.Name, this.CurrentLoginInfo.Username),
    }, authType);

   var cp = new ClaimsPrincipal(identity);
   var state = new AuthenticationState(cp);
   util.Log("GetAuthenticationStateAsync: " + this.CurrentLoginInfo.Username);
   return Task.FromResult(state);
  }
  else
  {
   util.Log("GetAuthenticationStateAsync: no user");
   var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
   return Task.FromResult(state);
  }

 }
}
