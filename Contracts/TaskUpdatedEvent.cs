namespace Contracts;

public record TaskUpdatedEvent
{
    public int TaskId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? AssignedTo { get; init; }
    public DateTime UpdatedOn { get; init; } = DateTime.UtcNow;
}