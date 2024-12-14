using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class Equipe
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        // Navigation property referencing the same entity type
        public Guid? ParentEquipeId { get; set; } // Foreign key to the parent Equipe

        [ForeignKey("ParentEquipeId")]
        public virtual Equipe? ParentEquipe { get; set; } // Reference to the parent Equipe

        public virtual ICollection<Equipe>? ChildEquipes { get; set; } // Collection of child Equipes
        public virtual ICollection<User>? Users { get; set; }
    }
}
