using Microsoft.EntityFrameworkCore;

public class QuestRepository : IQuestRepository
{
    private readonly LifeGrindDbContext context;
    public QuestRepository(LifeGrindDbContext _context)
    {
        context = _context;
    }

    public async Task<QuestEntity?> GetQuest(Guid questId)
    {
        return await context.Quests.FirstOrDefaultAsync(q => q.Id == questId);
    }
    public async Task<QuestEntity?> GetQuestByUserId(Guid userId)
    {
        return await context.Quests.FirstOrDefaultAsync(s => s.UserId == userId);
    }
    public  Task<List<QuestEntity>> GetQuestsByUserId(Guid userId)
    {
        return context.Quests.Where(s => s.UserId == userId).ToListAsync();
    }
    public async Task Add(QuestEntity Quest){
        await context.Quests.AddAsync(Quest);   
        await context.SaveChangesAsync();
    }
        
    public async Task Update(Guid QuestId,
        string title,string description,QuestDifficulty difficulty,
        int ExperienceReward,decimal CoinReward){
        await context.Quests.Where(s => s.Id == QuestId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Title,title)
                .SetProperty(u => u.Description,description)
                .SetProperty(u => u.Difficulty,difficulty)
                .SetProperty(u => u.CoinReward,CoinReward)
                .SetProperty(u => u.ExperienceReward,ExperienceReward)
            );
    }
    public async Task CompleteQuest(Guid QuestId,
        QuestStatus status,DateTime completedAt){
        await context.Quests.Where(s => s.Id == QuestId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Status,status)
                .SetProperty(u => u.CompletedAt,completedAt)
            );
            
    }
    public async Task CancelQuest(Guid QuestId,
        QuestStatus status,DateTime completedAt){
        await context.Quests.Where(s => s.Id == QuestId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Status,status)
                .SetProperty(u => u.CompletedAt,completedAt)
            );
            
    }
    public async Task Delete(Guid QuestId){
        await context.Quests.Where(u => u.Id == QuestId).ExecuteDeleteAsync();
    }
}