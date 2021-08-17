using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ColmanJira.Models;

namespace ColmanJira.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Task>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Task>()
                .HasOne(e => e.Status)
                .WithMany()
                .HasForeignKey(e => e.StatusId);
            modelBuilder.Entity<Task>()
                .HasOne(e => e.Type)
                .WithMany()
                .HasForeignKey(e => e.TypeId);
            modelBuilder.Entity<Task>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<TaskStatus>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<TaskStatus>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TaskType>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<TaskType>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
