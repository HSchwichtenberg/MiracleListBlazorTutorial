using Microsoft.AspNetCore.Components;
using MiracleListAPI;

namespace MiracleList.Client.Pages
{
 public partial class TaskEdit(AuthenticationManager am, MiracleListProxy proxy)
 {
  /// <summary>
  /// Zu bearbeitende Aufgabe
  /// </summary>
  [Parameter]
  public BO.Task Task { get; set; }

  /// <summary>
  /// Ereignis, wenn Aufgabe sich geändert hat. 
  /// True = Gespeichert. False = Cancel
  /// </summary>
  [Parameter]
  public EventCallback<bool> TaskHasChanged { get; set; }

  protected override async System.Threading.Tasks.Task OnInitializedAsync()
  {
  }

  // wenn Parameter gesetzt wird
  protected async override void OnParametersSet()
  {
  }

  protected async void Save()
  {
   await proxy.ChangeTaskAsync(this.Task, am.Token);
   await TaskHasChanged.InvokeAsync(true);
  }

  protected async void Cancel()
  {
   await TaskHasChanged.InvokeAsync(false);
  }
 } // end class TaskEdit
}
