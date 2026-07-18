public interface IQuestRepository
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task Add(QuestEntity Quest);
    Task Update(Guid QuestId,
        string title,string description,QuestDifficulty difficulty,
        int ExperienceReward,decimal CoinReward);
    Task Delete(Guid QuestId);
    Task UserFinishQuest(Guid QuestId,
        QuestStatus status,DateTime completedAt);
}