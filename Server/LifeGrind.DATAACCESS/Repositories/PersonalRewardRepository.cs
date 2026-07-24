using Microsoft.EntityFrameworkCore;

public class PersonalRewardRepository : IPersonalRewardRepository
{
    private readonly LifeGrindDbContext context;
    public PersonalRewardRepository(LifeGrindDbContext _context)
    {
        context = _context;
    }
    public  Task<List<PersonalRewardEntity>> GetRewardsByUserId(Guid userId){
        return context.PersonalRewards
            .AsNoTracking().Where(pR => pR.UserId == userId).ToListAsync();

    }
    public async Task AddPersonalReward(PersonalRewardEntity personalReward){
        await context.PersonalRewards.AddAsync(personalReward);
        await context.SaveChangesAsync();
    }
    public async Task UpdatePersonalReward(Guid pRewardId,UpdatePersonalRewardRequest request){
        await context.PersonalRewards.Where(p => p.Id == pRewardId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(p => p.Name,request.Name)
                .SetProperty(p => p.Description,request.Description)
                .SetProperty(p => p.Cost,request.Cost)
            );
    }
    public async Task DeletePersonalReward(Guid pRewardId){
         await context.PersonalRewards.Where(p => p.Id == pRewardId)
            .ExecuteDeleteAsync();
    }

    public async Task<PersonalRewardEntity?> GetPersonalRewardById(Guid pRewardId)
    {
        return await context.PersonalRewards.FirstOrDefaultAsync(p => p.Id == pRewardId);
    }
} 
