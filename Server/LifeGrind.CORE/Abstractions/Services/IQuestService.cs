public interface IQuestService
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task Add(Guid userId, CreateQuestRequest request);
    Task Update(Guid userId,Guid QuestId,UpdateQuestRequest request);
    Task Delete(Guid userId,Guid QuestId);
    Task CompleteQuest(Guid userId,Guid QuestId);
    Task CancelQuest(Guid userId,Guid QuestId);
}