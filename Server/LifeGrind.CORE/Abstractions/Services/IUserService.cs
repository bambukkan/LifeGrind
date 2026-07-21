public interface IUserService
{
    Task<UserEntity> GetMe(Guid userId);
    Task<string> Register(CreateUserRequest request);
    Task<string> Login(LoginUserRequest request);
    Task Update(Guid userId, UpdateUserRequest request);
    Task Delete(Guid userId);
}
