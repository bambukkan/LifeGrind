public interface IQuestRepository
{
    Task<List<QuestEntity>> GetQuests();
    Task<QuestEntity?> GetQuest(Guid questId);
    Task<QuestEntity?> GetQuestByUserId(Guid userId);
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task Add(QuestEntity Quest);
    Task Update(Guid QuestId,
        string title,string description,QuestDifficulty difficulty,
        int ExperienceReward,decimal CoinReward);
    Task Delete(Guid QuestId);
    Task CompleteQuest(Guid QuestId,QuestStatus status,DateTime completeAt);
    Task CancelQuest(Guid QuestId,QuestStatus status,DateTime completeAt);
}