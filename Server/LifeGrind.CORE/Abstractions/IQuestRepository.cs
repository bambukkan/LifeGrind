public interface IQuestRepository
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task<QuestEntity> Add();
    
    Task<QuestEntity> Update();
    Task<QuestEntity> Delete();
    Task UserFinishRequest(Guid QuestId,
        QuestStatus status,DateTime completedAt);
}