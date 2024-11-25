using System.Diagnostics;
using ITVisions.Blazor;
using MiracleListAPI;

namespace MiracleList;

public class AuthenticationManager(MiracleListAPI.MiracleListProxy proxy, BlazorUtil util)
{
 // alt;
 //MiracleListAPI.MiracleListProxy proxy = null;

 //public AuthenticationManger(MiracleListAPI.MiracleListProxy proxy)
 //{
 //  this.proxy = proxy;
 //}
 public LoginInfo CurrentLoginInfo { get; set; }
 public string Token => CurrentLoginInfo?.Token;
 public async Task<bool> Login(string username = null, string password = null)
 {
  try
  {
   var li = new LoginInfo();
   li.Username = username ?? "HolgerSchwichtenbergDDC";
   li.Password = password ?? "test";
   li.ClientID = "11111111-1111-1111-1111-111111111111";
   this.CurrentLoginInfo = await proxy.LoginAsync(li);

   util.Log("Login OK!");
   return true;
  }
  catch (Exception ex)
  {
   util.Log("Login failed: " + ex.Message);
   return false;
  }

 }
}
