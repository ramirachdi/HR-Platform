using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class ValidationTask
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Task))]
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public bool Validation { get; set; }
        public String? Cause { get; set; }
    }
}
