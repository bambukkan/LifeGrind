public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    public UserService(IUserRepository _userRepository)
    {
        userRepository = _userRepository;
    }
    public async Task<List<UserEntity>> GetUsers(){
        return await userRepository.GetUsers();
    }
    public async Task Add(UserEntity user){
        await userRepository.Add(user);
    }
    public async Task Update(Guid userId,string Name,
        string Email){
        await userRepository.Update(userId,Name,Email);
    }
    public async Task UpdateUserExpAndCoins(Guid userId,int TotalExperience,
        decimal Coins)
    {
        await userRepository.UpdateUserExpAndCoins(userId,TotalExperience,Coins);
    }
    public async Task Delete(Guid userId)
    {
        await userRepository.Delete(userId);
    }
}