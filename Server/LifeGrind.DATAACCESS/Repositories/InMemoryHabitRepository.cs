using LifeGrind.CORE.Entities;
using LifeGrind.CORE.Interfaces;

namespace LifeGrind.DATAACCESS.Repositories;

public sealed class InMemoryHabitRepository : IHabitRepository
{
    private readonly List<Habit> _habits = [];

    public Task<IReadOnlyCollection<Habit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Habit>>(_habits);
    }

    public Task<Habit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var habit = _habits.FirstOrDefault(item => item.Id == id);
        return Task.FromResult(habit);
    }

    public Task<Habit> CreateAsync(Habit habit, CancellationToken cancellationToken = default)
    {
        _habits.Add(habit);
        return Task.FromResult(habit);
    }
}
