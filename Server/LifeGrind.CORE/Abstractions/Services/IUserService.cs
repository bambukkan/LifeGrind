public interface IUserService
{
    Task<List<UserEntity>> GetUsers();
    Task Add(CreateUserRequest request);
    Task Update(Guid userId,UpdateUserRequest request);
    Task UpdateUserExpAndCoins(Guid userId,UpdateUserExpAndCoinsRequest request);
    Task Delete(Guid userId);
}