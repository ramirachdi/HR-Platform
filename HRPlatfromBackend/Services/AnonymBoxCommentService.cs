using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.Services
{
    public class AnonymBoxCommentService
    {
        public readonly AppDbContext _db;

        public AnonymBoxCommentService(AppDbContext db)
        {
            _db = db;
        }

        public List<AnonymBoxComment> display_comments()
        {
            return (_db.AnonymBoxComments.OrderByDescending(obj => obj.Date).ToList());
        }

        public void create_comment(string contenu)
        {
            AnonymBoxComment comment = new AnonymBoxComment(contenu, DateTime.Now);
            _db.AnonymBoxComments.Add(comment);
            _db.SaveChanges();

        }
    }
}
