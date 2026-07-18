public interface IQuestService
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task Add(QuestEntity Quest);
    Task Update(Guid QuestId,
        string title,string description,QuestDifficulty difficulty,
        int ExperienceReward,decimal CoinReward);
    Task Delete(Guid QuestId);
    Task UserFinishRequest(Guid QuestId,
        QuestStatus status,DateTime completedAt);
}