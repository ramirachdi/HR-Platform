using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class User : IdentityUser<Guid>
    {
        public string?  LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Adress { get; set; }
        public bool IsAdmin {  get; set; }
        public string? Status { get; set; }

        public DateTime ? DeletedAt { get; set; }

        public virtual ICollection<Equipe>? Equipes { get; set; }
        public virtual ICollection<Task>? Tasks { get; set; }  
        public virtual ICollection<Projet>? Projets { get; set; }
        public virtual ICollection<Meeting>? Meetings { get; set; }
        public virtual ICollection< Blame>? Blames { get; set; }
        public virtual ICollection<Medal>? Medals { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }





    }
}
