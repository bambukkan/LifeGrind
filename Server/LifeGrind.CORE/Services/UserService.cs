using System.Security.Authentication;
public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasher;
    public UserService(IUserRepository _userRepository,IPasswordHasher _passwordHasher)
    {
        userRepository = _userRepository;
        passwordHasher = _passwordHasher;
    }
    public async Task<List<UserEntity>> GetUsers(){
        return await userRepository.GetUsers();
    }
    public async Task Add(CreateUserRequest request){
        UserEntity user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHasher.GeneratePasswordHash(request.Password)
        };

        await userRepository.Add(user);
    }
    public async Task Update(Guid userId,UpdateUserRequest request){
        var user = await userRepository.GetUserById(userId);

        if (user == null || !passwordHasher.VerifyPassword(request.oldPassword, user.PasswordHash))
        {
            throw new InvalidCredentialException();
        }
        var newPasswordHash = passwordHasher.GeneratePasswordHash(request.newPassword);
        await userRepository.Update(userId,request.Name,request.Email,
        newPasswordHash); // Для имени и имейла будет все сделано во FluentValidation
    }
    public async Task UpdateUserExpAndCoins(Guid userId,UpdateUserExpAndCoinsRequest request)
    {
        // Тут пока не придумал какую валидацию с опытом и коинами сделами, ну в FV будет
        await userRepository.UpdateUserExpAndCoins(userId,request.TotalExperience,request.Coins);
    }
    public async Task Delete(Guid userId)
    {
        await userRepository.Delete(userId);
    }
}