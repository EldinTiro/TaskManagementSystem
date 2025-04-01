using TaskManagementSystem.API.Data;
using TaskManagementSystem.API.Repository;
using TaskStatus = TaskManagementSystem.API.Models.Enum.TaskStatus;

namespace TaskManagementSystem.Tests;

public class TaskTests
{
    private readonly TaskManagementSystemDbContext _context;
    public TaskTests()
    {
        _context = ContextGenerator.Generate();
    }

    #region Task Tests
    [Fact]
    public async Task CreateTask()
    {
        //Arrange
        var taskRepository = new TasksRepository(_context);
        
        //Act
        await taskRepository.AddAsync(new TaskEntity());

        //Assert
        Assert.Single(_context.Tasks);
    }
    
    [Fact]
    public async Task DeleteTask()
    {
        //Arrange
        var taskRepository = new TasksRepository(_context);
        
        //Act
        var task = await taskRepository.AddAsync(new TaskEntity());
        await taskRepository.DeleteAsync(task.Id);

        //Assert
        Assert.Empty(_context.Tasks);
    }
    
    [Theory]
    [InlineData("Discord", TaskStatus.InProgress)]
    [InlineData("MS Paint", TaskStatus.NotStarted)]
    [InlineData("Slack", TaskStatus.Completed)]
    public async Task UpdateTask(string newTaskName, TaskStatus newTaskStatus)
    {
        //Arrange
        var taskRepository = new TasksRepository(_context);
        
        //Act
        var task = await taskRepository.AddAsync(new TaskEntity() { Id = 1, Name = "JetBrains Rider", Description = "IDE", Status = TaskStatus.Completed, AssignedTo = "John Doe"});
        task.Name = newTaskName;
        task.Status = newTaskStatus;
        
        await taskRepository.UpdateAsync(task);
        
        var updatedTask = await taskRepository.GetAsync(task.Id);

        //Assert
        Assert.NotNull(updatedTask);
        Assert.Equal(updatedTask.Name, newTaskName);
        Assert.Equal(updatedTask.Status, newTaskStatus);
    }
    #endregion
}