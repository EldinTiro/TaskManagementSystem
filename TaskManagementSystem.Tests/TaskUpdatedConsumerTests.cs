using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagementSystem.API.Consumer;
using TaskManagementSystem.API.Contracts;
using TaskManagementSystem.API.Data;
using TaskStatus = TaskManagementSystem.API.Models.Enum.TaskStatus;

namespace TaskManagementSystem.Tests;

public class TaskUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldUpdateTaskInDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TaskManagementSystemDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new TaskManagementSystemDbContext(options);

        var taskRepositoryMock = new Mock<ITaskRepository>();

        // Seed the database with a task
        var existingTask = new TaskEntity
        {
            Id = 10,
            Name = "Original Task",
            Description = "Original Description",
            Status = TaskStatus.InProgress,
            AssignedTo = "John Doe"
        };
        context.Tasks.Add(existingTask);
        await context.SaveChangesAsync();

        // Mock repository to return the seeded task
        taskRepositoryMock.Setup(repo => repo.GetAsync(10))
            .ReturnsAsync(existingTask);

        var consumer = new TaskUpdatedConsumer(context, taskRepositoryMock.Object);

        var taskUpdatedEvent = new TaskUpdatedEvent
        {
            TaskId = 10,
            Name = "Updated Task",
            Description = "Updated Description",
            Status = "Completed",
            AssignedTo = "Jane Doe",
            UpdatedOn = DateTime.UtcNow
        };

        var mockConsumeContext = new Mock<ConsumeContext<TaskUpdatedEvent>>();
        mockConsumeContext.Setup(x => x.Message).Returns(taskUpdatedEvent);

        // Act
        await consumer.Consume(mockConsumeContext.Object);

        // Assert
        var updatedTask = await context.Tasks.FirstOrDefaultAsync(t => t.Id == 10);
        Assert.NotNull(updatedTask);
        Assert.Equal("Description updated from consumer", updatedTask.Description);
    }
}