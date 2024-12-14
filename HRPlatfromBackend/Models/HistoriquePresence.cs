
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class HistoriquePresence
    {
        [ForeignKey(nameof(Models.User))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Models.Meeting))]
        public Guid MeetingId { get; set; }

        public Meeting Meeting { get; set; }    
        public bool Presence {  get; set; }
        public bool Confirmed { get; set; }
        public bool Denied { get; set; }
        public String? Cause { get; set; }
        public User User { get; internal set; }

        
    }
}
