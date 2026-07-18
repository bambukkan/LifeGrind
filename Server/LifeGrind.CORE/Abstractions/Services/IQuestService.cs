public interface IQuestService
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task Add(Guid userId, CreateQuestRequest request);
    Task Update(Guid QuestId,UpdateQuestRequest request);
    Task Delete(Guid QuestId);
    Task CompleteQuest(Guid QuestId);
    Task CancelQuest(Guid QuestId);
}