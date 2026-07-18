using LifeGrind.CORE.Entities;

namespace LifeGrind.CORE.Interfaces;

public interface IHabitRepository
{
    Task<IReadOnlyCollection<Habit>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Habit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Habit> CreateAsync(Habit habit, CancellationToken cancellationToken = default);
}
