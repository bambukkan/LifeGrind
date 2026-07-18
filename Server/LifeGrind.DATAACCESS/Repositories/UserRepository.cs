using Microsoft.EntityFrameworkCore;

public class UserRepository
{
    private readonly LifeGrindDbContext context;
    public UserRepository(LifeGrindDbContext _context)
    {
        context = _context;
    }
    public Task<List<UserEntity>> GetUsers()
    {
        return context.Users
            .AsNoTracking()
            .ToListAsync();
    }
    public Task<List<UserEntity>> GetUserSkills(){
        return context.Users
            .AsNoTracking()
            .Include(u => u.Skills)
            .ToListAsync();
    }
    public Task<List<UserEntity>> GetUserQuests(){
        return context.Users
            .AsNoTracking()
            .ToListAsync();
    }
    // Получить скиллы и квесты по юзер айди
    public async Task Add(UserEntity user){
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
    
    public async Task Update(Guid userId,string Name,
        string Email){
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name,Name)
                .SetProperty(u => u.Email,Email)
            );
    }
    //сделать изменение опыта и койнов, по идее
    public async Task UpdateUserExpAndCoins(Guid userId,string Name,
        string Email){
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name,Name)
                .SetProperty(u => u.Email,Email)
            );
    }
    public async Task Delete(Guid userId){
        await context.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
    }
}
