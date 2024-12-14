using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class AnonymBoxComment
    {
    public Guid Id { get; set; }
    public string? Contenu { get; set; }
    public DateTime? Date { get; set; }

    public AnonymBoxComment() { }

    public AnonymBoxComment(string? contenu, DateTime? date)
        {
            Contenu = contenu;
            Date = date;
        }

    }
}
