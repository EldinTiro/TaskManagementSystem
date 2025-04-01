namespace Contracts;

public record TaskCreatedEvent
{
    public int TaskId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? AssignedTo { get; init; }
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
}