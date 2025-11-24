using Microsoft.AspNetCore.Components;
using MiracleListAPI;

namespace MiracleList.Client.Pages;

public partial class Home(AuthenticationManager am, MiracleListProxy proxy, NavigationManager nav)
{
 //[Inject]
 //AuthenticationManager am { get; set; }
 //[Inject]
 //MiracleListProxy proxy { get; set; }

 bool IsActiveCategory(BO.Category c)
 {
  return Category != null && c.CategoryID == Category.CategoryID;
 }

 string? GetCategoryColor(BO.Category c)
 {
  return IsActiveCategory(c) ? "#E0EEFA" : null;
 }

 protected override void OnInitialized()
 {
  Util.Log(nameof(Home));
 }

 [PersistentState] // NEU in .NET 10.0
 public List<BO.Category> CategorySet { get; set; } = new();

 [PersistentState] // NEU in .NET 10.0
 public List<BO.Task> TaskSet { get; set; } = new();

 [PersistentState]
 public BO.Category Category { get; set; }

 [PersistentState]
 public BO.Task Task { get; set; }

 public string NewCategoryName { get; set; }
 public string NewTaskName { get; set; }

 protected override async Task OnInitializedAsync()
 {
  Util.Log("Login", await am.Login());
  Util.Log("Token", am.Token);

  if (CategorySet == null)
  {
   CategorySet = await proxy.CategorySetAsync(am.Token);
   if (CategorySet.Any()) await ShowTaskSet(CategorySet.First());
   Util.Log("Category Count", CategorySet.Count);
  }
 }

 async Task ShowTaskSet(BO.Category c)
 {
  this.Category = c;
  this.TaskSet = await proxy.TaskSetAsync(c.CategoryID, am.Token);
 }

 void ShowTaskDetails(BO.Task t)
 {
  this.Task = t;
 }

 private async Task CreateCategory()
 {
  var newCategory = await proxy.CreateCategoryAsync(this.NewCategoryName, am.Token);
  this.CategorySet.Add(newCategory);
 }

 private async Task CreateTask()
 {
  var newTask = new BO.Task();
  newTask.Title = NewTaskName;
  newTask.Due = DateTime.Now.AddDays(1);
  newTask.CategoryID = this.Category.CategoryID;
  newTask = await proxy.CreateTaskAsync(newTask, am.Token);
  this.TaskSet.Add(newTask);
 }

 private async Task SaveDone(BO.Task t)
 {
  await proxy.ChangeTaskDoneAsync(t.TaskID, t.Done, am.Token);
 }

 public bool UIShouldRender { get; set; } = true;
 private async Task TaskChanged(bool args)
 {
  this.UIShouldRender = false;
  if (args) await proxy.ChangeTaskAsync(this.Task, am.Token);
  else await ShowTaskSet(this.Category);
  this.Task = null;
  this.UIShouldRender = true;

 }

 protected override bool ShouldRender()
 {
  return this.UIShouldRender;
 }
}
