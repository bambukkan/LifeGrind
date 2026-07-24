public interface IPersonalRewardService
{
    Task<List<PersonalRewardEntity>> GetRewardsByUserId(Guid userId);
    Task AddPersonalReward(Guid userId,CreatePersonalRewardRequest request);
    Task UpdatePersonalReward(Guid userId, Guid pRewardId, UpdatePersonalRewardRequest request);
    Task DeletePersonalReward(Guid userId, Guid pRewardId);
    Task PurchasePersonalReward(Guid pRewardId,Guid userId);
}
