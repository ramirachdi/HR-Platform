using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    [NotMapped]
    public class ValidationTaskForm
    {

        public Guid TaskId { get; set; }
        

        public ValidationTaskForm( Guid taskId)
        {
            
            TaskId = taskId;
            
        }
    }
}
