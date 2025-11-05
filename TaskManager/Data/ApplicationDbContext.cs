using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Configurations;
using TaskManager.Data.Entites;

namespace TaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
        }
        public virtual DbSet<TaskItem> TaskItems { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskItemConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
