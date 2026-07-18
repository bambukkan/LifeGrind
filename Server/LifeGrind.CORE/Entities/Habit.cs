namespace LifeGrind.CORE.Entities;

public sealed class Habit
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Title { get; init; }

    public string? Description { get; init; }

    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
}
