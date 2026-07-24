public interface IUserRepository
{
    Task<UserEntity?> GetMe(Guid userId);
    Task<UserEntity?> GetUserById(Guid userId);
    Task Add(UserEntity user);

    Task<UserEntity?> GetUserByEmail(string Email);
    Task Update(Guid userId,string Name,
        string Email,string newPasswordHash);
    Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins);
    Task Delete(Guid userId);
    Task<bool> TryPurchaseReward(Guid userId, decimal costReward);
}
