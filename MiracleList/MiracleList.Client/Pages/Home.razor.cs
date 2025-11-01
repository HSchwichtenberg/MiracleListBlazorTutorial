
using ITVisions.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MiracleListAPI;

namespace MiracleList.Client.Pages;

public partial class Home
{

 protected override void OnInitialized()
 {
  Util.Log(nameof(Home));
 }

}