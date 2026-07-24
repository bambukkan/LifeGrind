public interface IPersonalRewardRepository
{
    Task<List<PersonalRewardEntity>> GetRewardsByUserId(Guid userId);
    Task AddPersonalReward(PersonalRewardEntity personalReward);
    Task UpdatePersonalReward(Guid pRewardId,UpdatePersonalRewardRequest request);
    Task DeletePersonalReward(Guid pRewardId);
    Task<PersonalRewardEntity?> GetPersonalRewardById(Guid pRewardId);
}
