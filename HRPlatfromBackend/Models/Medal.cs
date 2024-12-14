using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class Medal
    { 
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }

       [ ForeignKey (nameof(User))]
        public Guid? UserId { get; set; }
        public virtual User? User { get; set; }  

    }
}
