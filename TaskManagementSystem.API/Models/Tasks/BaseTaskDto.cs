using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskManagementSystem.API.Models.Enum.TaskStatus;

namespace TaskManagementSystem.API.Models.Tasks;

public class BaseTaskDto
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public string? AssignedTo { get; set; }
}