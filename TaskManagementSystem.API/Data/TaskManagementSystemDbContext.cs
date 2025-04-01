using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.API.Data.Configurations;

namespace TaskManagementSystem.API.Data;

public class TaskManagementSystemDbContext: DbContext
{
    public TaskManagementSystemDbContext(DbContextOptions options): base(options)
    {
        
    }

    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
    }
}