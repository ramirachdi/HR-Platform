using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using ProjetNET.Models;
using Bogus;

namespace ProjetNET.Controllers
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<HistoriquePresence> HistoriquePresences { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Blame> Blames { get; set; }
        public DbSet<AnonymBoxComment> AnonymBoxComments { get; set; }
        public DbSet<Medal> Medals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ValidationTask> ValidationTasks { get; set; }
        public DbSet<PasswordRecovery> PasswordRecoveries { get; set; }

        public AppDbContext(DbContextOptions options)
         : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistoriquePresence>()
               .HasKey(h => new { h.UserId, h.MeetingId });

            modelBuilder.Entity<User>()
                .HasMany(e => e.Meetings)
                .WithMany(e => e.Users)
                .UsingEntity<HistoriquePresence>();

            modelBuilder.Entity<ValidationTask>()
              .HasKey(h => new { h.UserId, h.TaskId });

            modelBuilder.Entity<User>()
              .HasMany(e => e.Tasks)
              .WithMany(e => e.Users)
              .UsingEntity<ValidationTask>();

            modelBuilder.Entity<Equipe>()
                .HasMany(e => e.ChildEquipes)
                .WithOne(e => e.ParentEquipe)
                .HasForeignKey(e => e.ParentEquipeId)
                .IsRequired(false);

       



        }



    }
}
