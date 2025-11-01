using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Data.Entites;

namespace TaskManager.Data.Configurations
{
    public class TaskItemConfig : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable(nameof(TaskItem));
            builder.HasKey(x => x.Id);
        }
    }
}
