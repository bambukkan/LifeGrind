using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
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
    public async Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins){ // Нужно будет потом DTO для изменения
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.TotalExperience,TotalExperience)
                .SetProperty(u => u.Coins,Coins)
            );
    }
    public async Task Delete(Guid userId){
        await context.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
    }
}
