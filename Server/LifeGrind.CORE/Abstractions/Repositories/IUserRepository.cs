public interface IUserRepository
{
    Task<List<UserEntity>> GetUsers();
    Task<UserEntity> Add(UserEntity user);
    Task<UserEntity> Update(Guid userId,string Name,
        string Email);
    Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins);
    Task<UserEntity> Delete(Guid userId);
}