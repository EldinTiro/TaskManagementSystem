using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskStatus = TaskManagementSystem.API.Models.Enum.TaskStatus;

namespace TaskManagementSystem.API.Data.Configurations;

public class TaskConfiguration: IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasData(
            new TaskEntity
            {
                Id = 1,
                Name = "PowerPoint",
                Description = "Microsoft PowerPoint",
                AssignedTo = "John Doe",
                Status = TaskStatus.InProgress,
            },
            new TaskEntity
            {
                Id = 2,
                Name = "Word",
                Description = "Microsoft Word",
                AssignedTo = "John Doe",
                Status = TaskStatus.Completed,
            });
    }
}