using Microsoft.EntityFrameworkCore;

public class SkillRepository : ISkillRepository
{
    private readonly LifeGrindDbContext context;

    public SkillRepository(LifeGrindDbContext _context)
    {
        context = _context;
    }


    public async Task<SkillEntity?> GetSkill(Guid skillId)
    {
        return await context.Skills.FirstOrDefaultAsync(s => s.Id == skillId);
    }


    public Task<List<SkillEntity>> GetSkillsByUserId(Guid userId)
    {
        return context.Skills.Where(s => s.UserId == userId).ToListAsync();
    }

    public async Task Add(SkillEntity skill)
    {
        await context.Skills.AddAsync(skill);
        await context.SaveChangesAsync();
    }

    public async Task Update(Guid skillId,
        string name, string description, int experience)
    {
        await context.Skills.Where(s => s.Id == skillId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name, name)
                .SetProperty(u => u.Description, description)
                .SetProperty(u => u.Experience, experience)
            );
    }

    public async Task AddExperience(Guid skillId, int experience)
    {
        await context.Skills.Where(s => s.Id == skillId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Experience, u => u.Experience + experience)
            );
    }

    public async Task Delete(Guid skillId)
    {
        await context.Skills.Where(u => u.Id == skillId).ExecuteDeleteAsync();
    }
}
