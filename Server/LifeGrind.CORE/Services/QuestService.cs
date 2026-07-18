using LifeGrind.CORE.Exceptions;

public class QuestService : IQuestService
{
    private readonly IQuestRepository questRepository;
    private readonly IUserRepository userRepository;
    public QuestService(IQuestRepository _QuestRepository,IUserRepository _userRepository )
    {
        questRepository = _QuestRepository;
        userRepository = _userRepository;
    }
    public async Task<List<QuestEntity>> GetQuests(){
        return await questRepository.GetQuests();
    }
    public async Task<List<QuestEntity>> GetQuestsByUserId(Guid userId){
        return await questRepository.GetQuestsByUserId(userId);
    }   
    public async Task Add(Guid userId,CreateQuestRequest request)
    {
        var quest = new QuestEntity()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,  
            Description = request.Description,  
            Difficulty = request.Difficulty,  
            ExperienceReward = request.ExperienceReward,
            CoinReward = request.CoinReward  ,
            SkillId = request.SkillId,
            UserId = userId
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
    public async Task CompleteQuest(Guid QuestId) // статус либо завершен либо отменен здесь
    {
        QuestEntity? quest = await questRepository.GetQuest(QuestId);
        if(quest == null)
        {
            throw new QuestNotExistException();
        }
        if(quest.Status == QuestStatus.Completed)
        {
            throw new QuestHasAlreadyCompletedException();
        }
        if(quest.Status == QuestStatus.Cancelled)
        {
            throw new QuestHasCancelled();
        }

        await userRepository.UpdateUserExpAndCoins(
            quest.UserId,quest.ExperienceReward,quest.CoinReward
        );
        DateTime completedAt = DateTime.UtcNow;
        await questRepository.CompleteQuest(QuestId,QuestStatus.Completed,completedAt);
    }
    public async Task CancelQuest(Guid questId)
    {
        QuestEntity? quest = await questRepository.GetQuest(questId);
        if (quest == null)
            throw new QuestNotExistException();

        if (quest.Status == QuestStatus.Completed)
            throw new QuestHasAlreadyCompletedException();

        if (quest.Status == QuestStatus.Cancelled)
            throw new QuestHasCancelled();
        await questRepository.CancelQuest(
            questId,
            QuestStatus.Cancelled,
            DateTime.UtcNow
        );
    }
}