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
    public async Task Add(QuestEntity Quest)
    {
        await questRepository.Add(Quest);
    }
    public async Task Update(Guid QuestId,
        string title,string description,QuestDifficulty difficulty,
        int ExperienceReward,decimal CoinReward)
    {
        await questRepository.Update(QuestId,
        title,description,difficulty,ExperienceReward,CoinReward);
    }
    public async Task Delete(Guid QuestId){
        await questRepository.Delete(QuestId);
    }
    public async Task UserFinishRequest(Guid QuestId,
        QuestStatus status,DateTime completedAt)
    {
        await questRepository.UserFinishRequest(QuestId,status,completedAt);
    }
}