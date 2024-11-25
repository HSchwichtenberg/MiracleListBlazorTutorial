using Microsoft.AspNetCore.Components;

namespace MiracleList.Client.Pages;

public partial class TaskEdit
{

 [Parameter]
 public BO.Task Task { get; set; }

 [Parameter]
 public EventCallback<bool> TaskHasChanged { get; set; }

 public void Save()
 {
  TaskHasChanged.InvokeAsync(true);
 }

 public void Cancel()
 {
  TaskHasChanged.InvokeAsync(false);
 }


}
