namespace ProjetNET.Models
{
    public class Meeting
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }


        public virtual ICollection<User>? Users { get; set; }


    }
}
