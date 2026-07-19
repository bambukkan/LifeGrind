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
    public Task<UserEntity?> GetUserById(Guid userId)
    {
        return context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
    public async Task<UserEntity?> GetUserByEmail(string Email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }
    public async Task Add(UserEntity user){
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
    
    public async Task Update(Guid userId,string Name,
        string Email,string newPasswordHash){
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name,Name)
                .SetProperty(u => u.Email,Email)
                .SetProperty(u => u.PasswordHash,newPasswordHash)
            );
    }
    public async Task UpdateUserExpAndCoins(Guid userId,int ExperienceReward,
        decimal Coins){ // Нужно будет потом DTO для изменения
        await context.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.TotalExperience,u => u.TotalExperience+ExperienceReward)
                .SetProperty(u => u.Coins,u => u.Coins + Coins)
            );
    }
    public async Task Delete(Guid userId){
        await context.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
    }
}
