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

 [PersistentState]
 public List<BO.Category> CategorySet { get; set; } = new();
 [PersistentState]
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

  if (CategorySet == null || CategorySet.Count == 0)
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

 async Task NewCategory()
 {
  var newCategory = await proxy.CreateCategoryAsync(this.NewCategoryName, am.Token);
  CategorySet.Add(newCategory);
  this.NewCategoryName = "";
 }

 async Task NewTask()
 {
  var newTask = new BO.Task();
  newTask.Title = this.NewTaskName;
  newTask.CategoryID = this.Category.CategoryID;   //alternativ: newTask.Category = this.Category;
  newTask.Due = DateTime.Now.AddDays(1);
  await proxy.CreateTaskAsync(newTask, am.Token);
  TaskSet.Add(newTask);
  this.NewTaskName = "";

 }

 public async Task DoneChanged(BO.Task t)
 {
  try
  {
   await proxy.ChangeTaskDoneAsync(t.TaskID, t.Done, am.Token);
  }
  catch (Exception ex)
  {
   Util.Log("Fehler: " + ex.Message);
  }
 }

}
