public interface IUserRepository
{
    Task<List<UserEntity>> GetUsers();
    Task<UserEntity?> GetUserById(Guid userId);
    Task Add(UserEntity user);

    Task<UserEntity?> GetUserByEmail(string Email);
    Task Update(Guid userId,string Name,
        string Email,string newPasswordHash);
    Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins);
    Task Delete(Guid userId);
}