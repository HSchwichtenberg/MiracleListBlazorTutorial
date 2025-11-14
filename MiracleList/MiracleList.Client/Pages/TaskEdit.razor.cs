using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MiracleList.Client;

namespace MiracleList.Client.Pages
{
 public partial class TaskEdit
 {
  [Parameter] // zu bearbeitende Aufgabe
  public BO.Task Task { get; set; }

  [Parameter] // Ereignis, wenn Aufgabe sich geändert hat
  public EventCallback<bool> TaskHasChanged { get; set; }

  [Inject]
  MiracleListAPI.MiracleListProxy proxy { get; set; } = null;

  [Inject]
  AuthenticationManager am { get; set; }

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

  private async Task GetTask(int id)
  {
   this.Task = await proxy.TaskAsync(id, am.Token);
  }
 } // end class TaskEdit
}
