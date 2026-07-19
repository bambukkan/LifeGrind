public interface IUserService
{
    Task<List<UserEntity>> GetUsers();
    Task<string> Register(CreateUserRequest request);
    Task<string> Login(LoginUserRequest request);
    Task Update(Guid userId,UpdateUserRequest request);
    Task UpdateUserExpAndCoins(Guid userId,UpdateUserExpAndCoinsRequest request);
    Task Delete(Guid userId);
}