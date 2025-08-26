using Microsoft.EntityFrameworkCore;

namespace TodoDAL
{
    public class TodoDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TodoDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(256);

                entity.Property(e => e.Priority)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsDone)
                    .HasDefaultValue(false);
            });
        }
    }
}
