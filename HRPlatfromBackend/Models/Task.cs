namespace ProjetNET.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? DeadLine { get; set; }

        public virtual ICollection<User>? Users { get; set; }   


    }
}
