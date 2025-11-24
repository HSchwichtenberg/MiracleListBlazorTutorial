using Microsoft.AspNetCore.Components;

namespace MiracleList.Client.Pages;

public partial class TaskEdit
{
 [Parameter]
 public BO.Task CurrentTask { get; set; }
 [Parameter]
 public EventCallback<bool> TaskChanged { get; set; }

 private async Task Cancel()
 {
  await TaskChanged.InvokeAsync(false);
 }

 private async Task Save()
 {
  await TaskChanged.InvokeAsync(true);
 }
}