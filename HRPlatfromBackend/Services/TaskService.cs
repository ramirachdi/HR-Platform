using ProjetNET.Controllers;
using ProjetNET.Models;
  
namespace ProjetNET.Services
{
    public class TaskService
    {
        public readonly AppDbContext _db;
        public TaskService(AppDbContext db)
        {
            _db = db;
        }
        public List<Models.Task> GetAllTasks()
        {
            List<Models.Task> tasks = _db.Tasks.OrderByDescending(obj => obj.DeadLine).ToList();
            return (tasks);
        }
        public List<Models.Task> GetCurrentTasks()
        {
            List<Models.Task> tasks = _db.Tasks.Where(obj => obj.Status == "En cours").OrderByDescending(obj => obj.DeadLine).ToList();
            return (tasks);
        }
        public List<Models.Task> GetCurrentTasksByUserId(Guid userId)
        {
            var taskvalidations = _db.ValidationTasks.Where(c => c.UserId == userId).ToList();
    
            List<Models.Task> tasks = new List<Models.Task>();

            foreach (var taskvalidation in taskvalidations)
            {
                Console.WriteLine(taskvalidation.TaskId);
                using (var reader = _db.Tasks.Where(c => c.Id == taskvalidation.TaskId && c.Status == "En cours").Take(1).GetEnumerator())
                {
                    if (reader.MoveNext())
                    {
                        Console.WriteLine("adding task to return");
                        tasks.Add(reader.Current);
                    }
                }
            }
            return tasks;
        }

        public void CreateTask(Models.Task taskForm)
        {
            Models.Task task = new Models.Task()
            {
                DeadLine = taskForm.DeadLine,
                Description = taskForm.Description,
                Name = taskForm.Name,
                Status = "En cours",
                Users = new List<Models.User>()
            };

            _db.Tasks.Add(task);

            foreach (var userId in taskForm.Users)
            {
                var user = _db.Users.FirstOrDefault(c => c.Id == userId.Id);

                if (user != null)
                {
                    // Check if a ValidationTask with the same UserId and TaskId exists
                    var existingValidationTask = _db.ValidationTasks.FirstOrDefault(vt => vt.UserId == userId.Id && vt.TaskId == task.Id);

                    // If the ValidationTask doesn't already exist, create a new one
                    if (existingValidationTask == null)
                    {
                        var taskValidation = new Models.ValidationTask()
                        {
                            TaskId = task.Id,
                            UserId = userId.Id,
                            Validation = false,
                        };
                        _db.ValidationTasks.Add(taskValidation);
                    }
                }
            }

            _db.SaveChanges();
        }



    }
}
