using System.Net;
using ITVisions;
using ITVisions.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MiracleListAPI;

namespace MiracleList.Client.Pages;

public partial class Home(MiracleList.AuthenticationManager am, MiracleListProxy proxy, BlazorUtil Util)
{
 private PersistingComponentStateSubscription persistingSubscription;

 // alt:
 //[Inject]
 //public AuthenticationManager am { get; set; }



 private Task Persist()
 {
  ApplicationState.PersistAsJson(nameof(categorySet), categorySet);

  return Task.CompletedTask;
 }

 public List<BO.Category> categorySet { get; set; } = new();
 public List<BO.Task> taskSet { get; set; } = new();
 public BO.Category? category { get; set; } = null;
 public BO.Task? task { get; set; } = null;

 public string newCategoryName { get; set; } = "";
 public string newTaskTitle { get; set; } = "";

 protected override bool ShouldRender() => shouldRender;
 private bool shouldRender = false;

 protected override void OnInitialized()
 {
  Util.Log(nameof(Home) + ": " + this.RendererInfo.Name);
  //if (!this.RendererInfo.IsInteractive) shouldRender = false;
  persistingSubscription = ApplicationState.RegisterOnPersisting(Persist);
 }

 protected override async Task OnInitializedAsync()
 {

  bool r = await am.Login();
  if (r)
  {
   Util.Log("Das Token für alle weiteren Operationen ist: " + am.Token);
   // Liste der Kategorien laden
   //if (this.RendererInfo.IsInteractive)
   //{
   if (!ApplicationState.TryTakeFromJson<List<BO.Category>>(
           nameof(categorySet), out var restoredCategorySet))
   {
    Util.Log(this.RendererInfo.Name + "-> Lade Kategorien...");
    categorySet = await proxy.CategorySetAsync(am.Token);
    shouldRender = true;
   }
   else
   {
    Util.Log(this.RendererInfo.Name + "-> Kategorien aus PersistentState..." + restoredCategorySet.Count);
    categorySet = restoredCategorySet;
    shouldRender = true;
   }
   //}
  }
 }


 public async Task ShowTaskSet(BO.Category cat)
 {
  this.category = cat;
  this.task = null;
  taskSet = await proxy.TaskSetAsync(cat.CategoryID, am.Token);
 }

 public void ShowTaskDetails(BO.Task t)
 {
  if (t != this.task)
  {
   this.task = t;
  }
  else
  {
   this.task = null;
  }
  
 }

 public async Task NewCategory_KeyUp(KeyboardEventArgs args)
 {
  if (args.Key == "Enter")
  {
   if (!String.IsNullOrEmpty(newCategoryName))
   {
    var newCategory = await proxy.CreateCategoryAsync(newCategoryName, am.Token);
    categorySet.Add(newCategory);
    newCategoryName = "";
   }
  }
 }

 public async Task NewTask_KeyUp(KeyboardEventArgs args)
 {
  if (args.Key == "Enter")
  {
   if (!String.IsNullOrEmpty(newTaskTitle))
   {
    BO.Task t = new BO.Task();
    t.Title = newTaskTitle;
    t.CategoryID = category.CategoryID;

    var newTask = await proxy.CreateTaskAsync(t, am.Token);
    taskSet.Add(newTask);
    newTaskTitle = "";
   }
  }
 }

 public async Task ChangeTaskAsync(BO.Task t)
 {
  await proxy.ChangeTaskDoneAsync(t.TaskID, t.Done, am.Token);
 }

 public async Task TaskHasChanged(bool save)
 {
  if (save)
  {
   await proxy.ChangeTaskAsync(this.task, am.Token);
  }
  else
  {
   await ShowTaskSet(this.category);
  }

 }

}