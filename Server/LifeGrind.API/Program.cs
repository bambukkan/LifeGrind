using LifeGrind.CORE.Entities;
using LifeGrind.CORE.Interfaces;
using LifeGrind.DATAACCESS.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHabitRepository, InMemoryHabitRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/api/habits", async (IHabitRepository repository, CancellationToken cancellationToken) =>
{
    var habits = await repository.GetAllAsync(cancellationToken);
    return Results.Ok(habits);
})
.WithName("GetHabits");

app.MapGet("/api/habits/{id:guid}", async (Guid id, IHabitRepository repository, CancellationToken cancellationToken) =>
{
    var habit = await repository.GetByIdAsync(id, cancellationToken);
    return habit is null ? Results.NotFound() : Results.Ok(habit);
})
.WithName("GetHabitById");

app.MapPost("/api/habits", async (CreateHabitRequest request, IHabitRepository repository, CancellationToken cancellationToken) =>
{
    var habit = new Habit
    {
        Title = request.Title,
        Description = request.Description
    };

    var createdHabit = await repository.CreateAsync(habit, cancellationToken);
    return Results.Created($"/api/habits/{createdHabit.Id}", createdHabit);
})
.WithName("CreateHabit");

app.Run();

internal sealed record CreateHabitRequest(string Title, string? Description);
