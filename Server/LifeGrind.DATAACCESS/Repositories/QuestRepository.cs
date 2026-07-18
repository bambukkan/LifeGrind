public interface IQuestRepository
{
    Task<List<QuestEntity>> GetQuests();
    Task<QuestEntity> Add();
    
    Task<QuestEntity> Update();
    Task<QuestEntity> Delete();
}