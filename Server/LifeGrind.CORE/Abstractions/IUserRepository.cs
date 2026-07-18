public interface IUserRepository
{
    Task<List<UserEntity>> GetUsers();
    Task<UserEntity> Add();
    
    Task<UserEntity> Update();
    Task<UserEntity> Delete();
}