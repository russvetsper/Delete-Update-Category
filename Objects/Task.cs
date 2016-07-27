using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private DateTime _dueDate;
    public Task(string Description, int CategoryId, DateTime DueDate ,int Id = 0)
    {
      _id = Id;
      _description = Description;
      _categoryId= CategoryId;
      _dueDate=DueDate;
    }

    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId() == newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        bool categoryEquality = (this.GetCategoryId() == newTask.GetCategoryId());
        bool dueDateEquality = (this.GetDueDate() == newTask.GetDueDate());
        return (idEquality && descriptionEquality&& categoryEquality && dueDateEquality);
      }
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }

    public void SetCategoryId(int newCategoryId)
    {
      _categoryId = newCategoryId;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetDescription()
    {
      return _description;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public DateTime GetDueDate()
    {
      return _dueDate;
    }

    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks ORDER BY due_date;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        int taskCategoryId = rdr.GetInt32(2);
        DateTime taskDueDate = rdr.GetDateTime(3);

        Task newTask = new Task(taskDescription,taskCategoryId,taskDueDate, taskId);
        allTasks.Add(newTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description,Category_id,due_date) OUTPUT INSERTED.id VALUES (@TaskDescription, @TaskCategoryId,@TaskDueDate);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@TaskDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@TaskCategoryId";
      categoryIdParameter.Value = this.GetCategoryId();


      SqlParameter DueDateParameter = new SqlParameter();
      DueDateParameter.ParameterName = "@TaskDueDate";
      DueDateParameter.Value = this.GetDueDate();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(categoryIdParameter);
      cmd.Parameters.Add(DueDateParameter);


      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      int foundTaskCategoryId = 0;
      DateTime foundTaskDueDate = new DateTime(1573, 1, 1);

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundTaskCategoryId = rdr.GetInt32(2);
        foundTaskDueDate=rdr.GetDateTime(3);

      }
      Task foundTask = new Task(foundTaskDescription,foundTaskCategoryId,foundTaskDueDate, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }
    public static List<Task> FindDueDate(DateTime date)
    {
      List<Task> newList=new List<Task>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE due_date < @DueDate;", conn);
      SqlParameter taskDueDateParameter = new SqlParameter();
      taskDueDateParameter.ParameterName = "@DueDate";
      taskDueDateParameter.Value = date;
      cmd.Parameters.Add(taskDueDateParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;
      int foundTaskCategoryId = 0;
      DateTime foundTaskDueDate = new DateTime(1573, 1, 1);

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundTaskCategoryId = rdr.GetInt32(2);
        foundTaskDueDate=rdr.GetDateTime(3);
        Task foundTask = new Task(foundTaskDescription,foundTaskCategoryId,foundTaskDueDate, foundTaskId);
        newList.Add(foundTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return newList;
    }

  }
}
