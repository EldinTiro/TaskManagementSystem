using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagementSystem.API.Consumer;
using TaskManagementSystem.API.Data;
using TaskStatus = TaskManagementSystem.API.Models.Enum.TaskStatus;

namespace TaskManagementSystem.Tests;

public class TaskCreatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldAddTaskToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TaskManagementSystemDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new TaskManagementSystemDbContext(options);
        var consumer = new TaskCreatedConsumer(context);

        var taskCreatedEvent = new TaskCreatedEvent
        {
            Name = "Test Task",
            Description = "Test Description",
            Status = "InProgress",
            AssignedTo = "John Doe",
            CreatedOn = DateTime.UtcNow
        };

        var mockConsumeContext = new Mock<ConsumeContext<TaskCreatedEvent>>();
        mockConsumeContext.Setup(x => x.Message).Returns(taskCreatedEvent);

        // Act
        await consumer.Consume(mockConsumeContext.Object);

        // Assert
        var addedTask = await context.Tasks.FirstOrDefaultAsync(t => t.Name == "Test Task");
        Assert.NotNull(addedTask);
        Assert.Equal("Test Description", addedTask.Description);
        Assert.Equal(TaskStatus.InProgress, addedTask.Status);
        Assert.Equal("John Doe", addedTask.AssignedTo);
    }
}