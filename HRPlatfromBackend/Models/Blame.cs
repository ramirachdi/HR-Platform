using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class Blame
    {
        public Guid Id { get; set; }
        public string? Object { get; set; }
        public string? Name { get; set; }
        public string? Contention { get; set; }
        public DateTime? DateCreation { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }


    }
}
