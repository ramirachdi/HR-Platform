using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    [NotMapped]
    public class TaskForm
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }
        public List<Guid> Users { get; set; }
        public TaskForm(string? name, string? description, DateTime? deadLine, List<Guid> users)
        {
            Name = name;
            Description = description;
            DeadLine = deadLine;
            Users = users;
        }
    }
}
