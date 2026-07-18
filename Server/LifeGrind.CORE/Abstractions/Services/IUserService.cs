public interface IUserService
{
    Task<List<UserEntity>> GetUsers();
    Task<UserEntity> Add();
    Task<UserEntity> Update();
    Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins);
    Task<UserEntity> Delete();
}