using Contracts;
using MassTransit;
using TaskManagementSystem.API.Contracts;
using TaskManagementSystem.API.Data;

namespace TaskManagementSystem.API.Consumer;

public sealed class TaskUpdatedConsumer: IConsumer<TaskUpdatedEvent>
{
    private readonly TaskManagementSystemDbContext _context;
    private readonly ITaskRepository _taskRepository;

    public TaskUpdatedConsumer(TaskManagementSystemDbContext context, ITaskRepository taskRepository)
    {
        _context = context;
        _taskRepository = taskRepository;
    }
    
    // We already updated the Task entity with regular CRUD operation, but let's update another one just to prove that event flow is working correctly
    public async Task Consume(ConsumeContext<TaskUpdatedEvent> context)
    {
        Console.WriteLine("Consuming TaskUpdatedEvent");
        
        var updatedTask = await _taskRepository.GetAsync(context.Message.TaskId);
        
        updatedTask.Description = "Description updated from consumer";
        
        _context.Tasks.Update(updatedTask);
        
        await _context.SaveChangesAsync();
        
        Console.WriteLine("TaskUpdatedEvent Consumed");
    }
}