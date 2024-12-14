namespace ProjetNET.Models
{
    public class Projet
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } 
        public string? Status { get; set; }

        public virtual ICollection<User>? Users { get; set; }   

    }
}
