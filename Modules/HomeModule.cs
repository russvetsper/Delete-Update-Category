using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
    Get["/"] = _ => {
      return View["index.cshtml"];
    };
    Get["/due-date"] = _ => {
      return View["dueDate_form.cshtml"];
    };

    Get["/duedate"] = _ => {
      List<Task> AllTasks = Task.FindDueDate(Request.Query["latest-date"]);
      return View["tasks.cshtml",AllTasks];
    };


    Get["/tasks"] = _ => {
    List<Task> AllTasks = Task.GetAll();
    return View["tasks.cshtml", AllTasks];
    };
    Get["/categories"] = _ => {
      List<Category> AllCategories = Category.GetAll();
      return View["categories.cshtml", AllCategories];
    };

    Get["/categories/new"] = _ => {
    return View["categories_form.cshtml"];
    };
    Post["/categories/new"] = _ => {
      Category newCategory = new Category(Request.Form["category-name"]);
      newCategory.Save();
      return View["success.cshtml"];
    };
    Get["/tasks/new"] = _ => {
      List<Category> AllCategories = Category.GetAll();
      return View["tasks_form.cshtml", AllCategories];
    };
    Post["/tasks/new"] = _ => {
      Task newTask = new Task(Request.Form["task-description"], Request.Form["category-id"], Request.Form["new-date"]);
      newTask.Save();
      return View["success.cshtml"];
    };

    Post["/tasks/delete"] = _ => {
      Task.DeleteAll();
      return View["cleared.cshtml"];
    };

    Get["/categories/{id}"] = parameters => {
      Dictionary<string, object> model = new Dictionary<string, object>();
      var SelectedCategory = Category.Find(parameters.id);
      var CategoryTasks = SelectedCategory.GetTasks();
      model.Add("category", SelectedCategory);
      model.Add("tasks", CategoryTasks);
      return View["category.cshtml", model];
    };

    Get["category/edit/{id}"] = parameters => {
      Category SelectedCategory = Category.Find(parameters.id);
      return View["category_edit.cshtml", SelectedCategory];
    };

    Patch["category/edit/{id}"] = parameters=>{
      Category SelectedCategory = Category.Find(parameters.id);
      SelectedCategory.Update(Request.Form["category-name"]);
      return View["success.cshtml"];
    };

    Get["category/delete/{id}"] = parameters => {
      Category SelectedCategory = Category.Find(parameters.id);
      return View["category_delete.cshtml", SelectedCategory];
    };
    Delete["category/delete/{id}"] = parameters => {
      Category SelectCategory = Category.Find(parameters.id);
      SelectCategory.Delete();
      return View["success.cshtml"];
    };



   }
  }
}
