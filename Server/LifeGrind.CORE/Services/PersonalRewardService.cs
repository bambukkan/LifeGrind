using LifeGrind.CORE.Exceptions;

public class PersonalRewardService : IPersonalRewardService
{
    private readonly IPersonalRewardRepository prRepository;
    private readonly IUserRepository userRepository;
    private readonly ITransactionManager transactionManager;

    public PersonalRewardService(
        IPersonalRewardRepository personalRewardRepository,
        IUserRepository _userRepository,
        ITransactionManager _transactionManager)
    {
        prRepository = personalRewardRepository;
        userRepository = _userRepository;
        transactionManager = _transactionManager;
    }

    public Task<List<PersonalRewardEntity>> GetRewardsByUserId(Guid userId)
    {
        return prRepository.GetRewardsByUserId(userId);
    }

    public async Task AddPersonalReward(Guid userId, CreatePersonalRewardRequest request)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new EntityNotFoundException("Пользователь не найден.");
        }

        var personalReward = new PersonalRewardEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Cost = request.Cost,
            UserId = userId
        };

        await prRepository.AddPersonalReward(personalReward);
    }

    public async Task UpdatePersonalReward(
        Guid userId,
        Guid pRewardId,
        UpdatePersonalRewardRequest request)
    {
        var reward = await GetOwnedPersonalReward(userId, pRewardId);
        await prRepository.UpdatePersonalReward(reward.Id, request);
    }

    public async Task DeletePersonalReward(Guid userId, Guid pRewardId)
    {
        var reward = await GetOwnedPersonalReward(userId, pRewardId);
        await prRepository.DeletePersonalReward(reward.Id);
    }

    public async Task PurchasePersonalReward(Guid pRewardId, Guid userId)
    {
        var reward = await GetOwnedPersonalReward(userId, pRewardId);
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new EntityNotFoundException("Пользователь не найден.");
        }

        await transactionManager.ExecuteAsync(async () =>
        {
            var purchased = await userRepository.TryPurchaseReward(userId, reward.Cost);
            if (!purchased)
            {
                throw new NotEnoughCoinsException();
            }
        });
    }

    private async Task<PersonalRewardEntity> GetOwnedPersonalReward(
        Guid userId,
        Guid pRewardId)
    {
        var reward = await prRepository.GetPersonalRewardById(pRewardId);
        if (reward == null)
        {
            throw new EntityNotFoundException("Личная награда не найдена.");
        }

        if (reward.UserId != userId)
        {
            throw new EntityNotFoundException("Нельзя получить доступ к чужой личной награде.");
        }

        return reward;
    }
}
