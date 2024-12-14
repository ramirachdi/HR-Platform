using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    [NotMapped]
    public class AnonymBoxCommentForm
    {
        
        public string? Contenu { get; set; }
        public AnonymBoxCommentForm(string? contenu)
        {
            Contenu = contenu;
        }
    }
}
