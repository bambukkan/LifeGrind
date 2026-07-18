public interface IUserRepository
{
    Task<List<UserEntity>> GetUsers();
    Task Add(UserEntity user);
    Task Update(Guid userId,string Name,
        string Email);
    Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins);
    Task Delete(Guid userId);
}