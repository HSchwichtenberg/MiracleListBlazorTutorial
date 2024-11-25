using System.Net;
using ITVisions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MiracleListAPI;

namespace MiracleList.Client.Pages;

public partial class Home(MiracleList.AuthenticationManager am, MiracleListProxy proxy)
{

 // alt:
 //[Inject]
 //public AuthenticationManager am { get; set; }

 protected override void OnInitialized()
 {
  Util.Log(nameof(Home));
 }

 public List<BO.Category> categorySet { get; set; } = new();
 public List<BO.Task> taskSet { get; set; }  = new();
 public BO.Category? category { get; set; } = null;
 public BO.Task? task { get; set; } = null;

 public string newCategoryName { get; set; } = "";
 public string newTaskTitle { get; set; } = "";

 protected override async Task OnInitializedAsync()
 {
  bool r = await am.Login();
  if (r)
  {
   Util.Log("Das Token für alle weiteren Operationen ist: " + am.Token);
   // Liste der Kategorien laden
   categorySet = await proxy.CategorySetAsync(am.Token);
  }
 }

 public async Task ShowTaskSet(BO.Category cat)
 {
  this.category = cat;
  taskSet = await proxy.TaskSetAsync(cat.CategoryID, am.Token);
 }

 public void ShowTaskDetails(BO.Task t)
 {
  this.task = t;
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
}