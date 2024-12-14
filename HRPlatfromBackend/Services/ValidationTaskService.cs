using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.Services
{
    public class ValidationTaskService
    {
        public readonly AppDbContext _db;
        public ValidationTaskService(AppDbContext db)
        {
            _db = db;
        }
        public void ValidateTask(ValidationTaskForm validationTaskForm) {
      
            var validations = _db.ValidationTasks.Where(x => x.TaskId == validationTaskForm.TaskId);
            foreach (var validationTask in validations)
            {
                validationTask.Validation = true;
            }
            _db.SaveChanges();
            var task=_db.Tasks.FirstOrDefault(x => x.Id == validationTaskForm.TaskId);
            if (task != null) { task.Status = "Validé"; }
            _db.SaveChanges();

        }
    }
}
