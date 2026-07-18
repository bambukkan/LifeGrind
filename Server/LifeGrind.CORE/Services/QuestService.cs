public class QuestService : IQuestService
{
    private readonly IQuestRepository questRepository;
    public QuestService(IQuestRepository _QuestRepository)
    {
        questRepository = _QuestRepository;
    }
    public async Task<List<QuestEntity>> GetQuests(){
        return await questRepository.GetQuests();
    }
    public async Task<List<QuestEntity>> GetQuestsByUserId(Guid userId){
        return await questRepository.GetQuestsByUserId(userId);
    }   
    public async Task Add(CreateQuestRequest request)
    {
        var quest = new QuestEntity()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,  
            Description = request.Description,  
            Difficulty = request.Difficulty,  
            ExperienceReward = request.ExperienceReward,
            CoinReward = request.CoinReward  
        }; //  Ну тут так же валидацйия с FV проверяться будет 
        await questRepository.Add(quest);
    }
    public async Task Update(Guid QuestId,
        UpdateQuestRequest request)
    {
        await questRepository.Update(QuestId,
        request.Title,request.Description,request.Difficulty,request.ExperienceReward,request.CoinReward);
    }
    public async Task Delete(Guid QuestId){
        await questRepository.Delete(QuestId);
    }
    public async Task UserFinishQuest(Guid QuestId,
    UpdateUserFinishQuestRequest request) // статус либо завершен либо отменен здесь
    {
        DateTime completedAt = DateTime.UtcNow;
        await questRepository.UserFinishQuest(QuestId,request.Status,completedAt);
    }
}