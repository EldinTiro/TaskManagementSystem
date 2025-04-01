using TaskManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Tests;

public static class ContextGenerator
{
    public static TaskManagementSystemDbContext Generate()
    {
        var optionBuilder = new DbContextOptionsBuilder<TaskManagementSystemDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new TaskManagementSystemDbContext(optionBuilder.Options);
    }
}