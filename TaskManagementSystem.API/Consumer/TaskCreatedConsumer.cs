using Contracts;
using MassTransit;
using TaskManagementSystem.API.Data;
using TaskStatus = TaskManagementSystem.API.Models.Enum.TaskStatus;

namespace TaskManagementSystem.API.Consumer;

public sealed class TaskCreatedConsumer: IConsumer<TaskCreatedEvent>
{
    private readonly TaskManagementSystemDbContext _context;

    public TaskCreatedConsumer(TaskManagementSystemDbContext context)
    {
        _context = context;
    }
    
    // We already persisted the Task entity with regular CRUD operation, but let's persist another one (with different id) just to prove that event flow is working correctly
    public async Task Consume(ConsumeContext<TaskCreatedEvent> context)
    {
        Console.WriteLine("Consuming TaskCreatedEvent");
        var task = new TaskEntity
        {
            Name = context.Message.Name,
            Description = context.Message.Description,
            Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), context.Message.Status),
            AssignedTo = context.Message.AssignedTo,
        };
        
        _context.Tasks.Add(task);
        
        await _context.SaveChangesAsync();
        
        Console.WriteLine("TaskCreatedEvent Consumed");
    }
}