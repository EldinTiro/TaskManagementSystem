using TaskManagementSystem.API.Contracts;
using TaskManagementSystem.API.Data;

namespace TaskManagementSystem.API.Repository;

public class TasksRepository : GenericRepository<TaskEntity>, ITaskRepository
{
    private readonly TaskManagementSystemDbContext _context;

    public TasksRepository(TaskManagementSystemDbContext context) : base(context)
    {
        _context = context;
    }
}