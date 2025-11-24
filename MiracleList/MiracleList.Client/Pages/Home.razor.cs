
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MiracleListAPI;

namespace MiracleList.Client.Pages;

public partial class Home
{
 [Inject]
 public AuthenticationManager am { get; set; }
 [Inject]
 public MiracleListProxy proxy { get; set; }

 [Inject]
 IJSRuntime js { get; set; }

 [Inject]
 NavigationManager nav { get; set; }

 protected override void OnInitialized()
 {
  Util.Log(nameof(Home));
 }

 public List<BO.Category> CategorySet { get; set; } = new();
 public List<BO.Task> TaskSet { get; set; } = new();
 public BO.Category? Category { get; set; }
 public BO.Task? Task { get; set; }

 string newCategoryName { get; set; } = "";
 string newTaskTitle { get; set; } = "";

 protected override async Task OnInitializedAsync()
 {
  Util.Log(this.RendererInfo.Name + "/" + nav.GetType().FullName + "/" + js.GetType().FullName);
  Util.Log("Login", await am.Login());
  Util.Log("Token", am.Token);

  await ShowCategorySet();
  if (this.CategorySet.Count > 0) await ShowTaskSet(CategorySet[0]);

  Util.Log("Category Count", CategorySet.Count);
 }

 async Task ShowCategorySet()
 {
  this.CategorySet = await proxy.CategorySetAsync(am.Token);
 }

 async Task ShowTaskSet(BO.Category? c)
 {
  if (c == null) return;
  this.Category = c;
  this.TaskSet = await proxy.TaskSetAsync(c.CategoryID, am.Token);
 }

 bool IsActiveCategory(BO.Category c)
 {
  return Category != null && c.CategoryID == Category.CategoryID;
 }

 string? GetCategoryColor(BO.Category c)
 {
  return IsActiveCategory(c) ? "#E0EEFA" : null;
 }

 void ShowTaskDetails(BO.Task t)
 {
  this.Task = t;
 }

 public bool UIShouldRender { get; set; } = true;
 public async Task TaskHasChanged(bool saved)
 {
  this.UIShouldRender = false;
  if (!saved) // Neuladen, um zu altem Zustand des Task-Objekts zu kommen! Alternative: Undo selbst implementieren!
  {
   await ShowTaskSet(this.Category);
  }
  else
  {
   //this.StateHasChanged(); // wäre im Fall true notwendig, wenn wir nicht noch eine Eigenschaft verändern, um das Rendering wieder anzustoßen, um den gespeicherten Wert anzuzeigen!
  }
  this.Task = null;
  this.UIShouldRender = true;
 }

 protected override bool ShouldRender()
 {
  return this.UIShouldRender;
 }

 public async Task NewCategory()
 {
  var newcategory = await proxy.CreateCategoryAsync(newCategoryName, am.Token);
  await ShowCategorySet();
  await ShowTaskSet(newcategory);
 }

 /// <summary>
 /// Alternative: Use Keyup instead of Keypress as the actual data binding did not yet happen when Keypress is fired
 /// </summary>
 public async Task NewCategory_Keyup(KeyboardEventArgs e)
 {
  if (e.Key == "Enter")
  {
   if (!String.IsNullOrEmpty(this.newCategoryName))
   {
    var newcategory = await proxy.CreateCategoryAsync(newCategoryName, am.Token);
    await ShowCategorySet();
    await ShowTaskSet(newcategory);
   }
  }
 }

 /// <summary>
 /// Use Keyup instead of Keypress as the actual data binding did not yet happen when Keypress is fired
 /// </summary>
 public async Task NewTask_Keyup(KeyboardEventArgs e)
 {
  if (e.Key == "Enter")
  {
   if (!String.IsNullOrEmpty(this.newTaskTitle))
   {
    if (string.IsNullOrEmpty(newTaskTitle)) return;
    var t = new BO.Task();
    t.TaskID = 0; // notwendig für Server, da der die ID vergibt
    t.Title = newTaskTitle;
    t.CategoryID = this.Category.CategoryID;
    t.Importance = BO.Importance.B;
    t.Created = DateTime.Now;
    t.Due = null;
    t.Order = 0;
    t.Note = "";
    t.Done = false;
    await proxy.CreateTaskAsync(t, am.Token);
    await ShowTaskSet(this.Category);
    this.newTaskTitle = "";
   }
  }
 }

 /// <summary>
 /// Ereignisbehandlung: Benutzer löscht Aufgabe
 /// </summary>
 public async System.Threading.Tasks.Task RemoveTask(BO.Task t)
 {
  await js.SetValueAsync("document.title", "Nachfrage");
  // Rückfrage (Browser-Dialog via JS!)
  if (!await js.InvokeAsync<bool>("confirm", "Remove Task #" + t.TaskID + ": " + t.Title + "?")) return;
  // Löschen via WebAPI-Aufruf
  await proxy.DeleteTaskAsync(t.TaskID, am.Token);
  // Liste der Aufgaben neu laden
  await ShowTaskSet(this.Category);
  // aktuelle Aufgabe zurücksetzen
  this.Task = null;
  await js.SetValueAsync("document.title", "MiracleList");
 }

 /// <summary>
 /// Ereignisbehandlung: Benutzer löscht Kategorie
 /// </summary>
 /// <param name="c">zu löschende Kategorie</param>
 public async System.Threading.Tasks.Task RemoveCategory(BO.Category c)
 {
  // Rückfrage (Browser-Dialog via JS!)
  if (!await js.InvokeAsync<bool>("confirm", "Remove Category #" + c.CategoryID + ": " + c.Name + "?")) return;
  // Löschen via WebAPI-Aufruf
  await proxy.DeleteCategoryAsync(c.CategoryID, am.Token);
  // Liste der Kategorien neu laden
  await ShowCategorySet();
  // aktuelle Category zurücksetzen
  this.Category = null;
 }
}