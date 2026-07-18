public interface IQuestService
{
    Task<List<QuestEntity>> GetQuests();
    Task<List<QuestEntity>> GetQuestsByUserId(Guid userId);
    Task Add(CreateQuestRequest request);
    Task Update(Guid QuestId,UpdateQuestRequest request);
    Task Delete(Guid QuestId);
    Task UserFinishQuest(Guid QuestId,UpdateUserFinishQuestRequest request);
}