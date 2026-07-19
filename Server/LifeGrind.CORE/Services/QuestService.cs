using LifeGrind.CORE.Exceptions;

public class QuestService : IQuestService
{
    private readonly IQuestRepository questRepository;
    private readonly IUserRepository userRepository;
    private readonly ISkillRepository skillRepository;

    public QuestService(
        IQuestRepository _QuestRepository,
        IUserRepository _userRepository,
        ISkillRepository _skillRepository)
    {
        questRepository = _QuestRepository;
        userRepository = _userRepository;
        skillRepository = _skillRepository;
    }

    public async Task<List<QuestEntity>> GetQuests()
    {
        return await questRepository.GetQuests();
    }

    public async Task<List<QuestEntity>> GetQuestsByUserId(Guid userId)
    {
        return await questRepository.GetQuestsByUserId(userId);
    }

    public async Task Add(Guid userId, CreateQuestRequest request)
    {
        var quest = new QuestEntity()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Difficulty = request.Difficulty,
            ExperienceReward = request.ExperienceReward,
            CoinReward = request.CoinReward,
            SkillId = request.SkillId,
            UserId = userId
        };

        await questRepository.Add(quest);
    }

    public async Task Update(Guid userId, Guid questId, UpdateQuestRequest request)
    {
        var quest = await GetOwnedQuest(userId, questId);

        await questRepository.Update(
            quest.Id,
            request.Title,
            request.Description,
            request.Difficulty,
            request.ExperienceReward,
            request.CoinReward);
    }

    public async Task Delete(Guid userId, Guid questId)
    {
        var quest = await GetOwnedQuest(userId, questId);

        await questRepository.Delete(quest.Id);
    }

    public async Task CompleteQuest(Guid userId, Guid questId)
    {
        var quest = await GetOwnedQuest(userId, questId);

        if (quest.Status == QuestStatus.Completed)
        {
            throw new QuestHasAlreadyCompletedException();
        }

        if (quest.Status == QuestStatus.Cancelled)
        {
            throw new QuestHasCancelled();
        }

        await userRepository.UpdateUserExpAndCoins(
            quest.UserId,
            quest.ExperienceReward,
            quest.CoinReward);

        await skillRepository.AddExperience(
            quest.SkillId,
            quest.ExperienceReward);

        await questRepository.CompleteQuest(
            quest.Id,
            QuestStatus.Completed,
            DateTime.UtcNow);
    }

    public async Task CancelQuest(Guid userId, Guid questId)
    {
        var quest = await GetOwnedQuest(userId, questId);

        if (quest.Status == QuestStatus.Completed)
        {
            throw new QuestHasAlreadyCompletedException();
        }

        if (quest.Status == QuestStatus.Cancelled)
        {
            throw new QuestHasCancelled();
        }

        await questRepository.CancelQuest(
            quest.Id,
            QuestStatus.Cancelled,
            DateTime.UtcNow);
    }

    private async Task<QuestEntity> GetOwnedQuest(Guid userId, Guid questId)
    {
        var quest = await questRepository.GetQuest(questId);
        if (quest == null)
        {
            throw new QuestNotExistException();
        }

        if (quest.UserId != userId)
        {
            throw new EntityNotFoundException("Attempt to access another user's quest.");
        }

        return quest;
    }
}
