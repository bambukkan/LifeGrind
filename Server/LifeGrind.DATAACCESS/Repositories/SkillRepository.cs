using Microsoft.EntityFrameworkCore;

public class SkillRepository : ISkillRepository
{
    private readonly LifeGrindDbContext context;
    public SkillRepository(LifeGrindDbContext _context)
    {
        context = _context;
    }
    public Task<List<SkillEntity>> GetSkills(){
        return context.Skills.ToListAsync();
    }
    public  Task<List<SkillEntity>> GetSkillsByUserId(Guid userId)
    {
        return context.Skills.Where(s => s.UserId == userId).ToListAsync();
    }
    public async Task Add(SkillEntity skill){
        await context.Skills.AddAsync(skill);   
        await context.SaveChangesAsync();
    }
        
    public async Task Update(Guid skillId,
        string name,string description,int experience){
        await context.Skills.Where(s => s.Id == skillId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name,name)
                .SetProperty(u => u.Description,description)
                .SetProperty(u => u.Experience,experience)
            );
    }
    public async Task Delete(Guid skillId){
        await context.Skills.Where(u => u.Id == skillId).ExecuteDeleteAsync();
    }
}