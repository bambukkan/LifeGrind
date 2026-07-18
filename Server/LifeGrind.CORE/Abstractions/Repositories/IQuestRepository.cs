public interface IQuestRepository
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task<QuestEntity> Add(QuestEntity Quest);
    Task<QuestEntity> Update(Guid QuestId,
        string title,string description,QuestDifficulty difficulty,
        int ExperienceReward,decimal CoinReward);
    Task<QuestEntity> Delete(Guid QuestId);
    Task UserFinishRequest(Guid QuestId,
        QuestStatus status,DateTime completedAt);
}