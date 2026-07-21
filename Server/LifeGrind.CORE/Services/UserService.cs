using LifeGrind.CORE.Exceptions;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtProvider jwtProvider;

    public UserService(IUserRepository _userRepository,
        IPasswordHasher _passwordHasher, IJwtProvider _jwtProvider)
    {
        userRepository = _userRepository;
        passwordHasher = _passwordHasher;
        jwtProvider = _jwtProvider;
    }

    public async Task<UserEntity> GetMe(Guid userId)
    {
        var user = await userRepository.GetMe(userId);
         if (user == null)
        {
            throw new EntityNotFoundException("Пользователь не найден");
        }
        return user;
    }

    public async Task<string> Register(CreateUserRequest request)
    {
        var existingUser = await userRepository.GetUserByEmail(request.Email);
        if(existingUser != null)
        {
            throw new EmailAlreadyExistsException();
        }
        UserEntity user = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHasher.GeneratePasswordHash(request.Password)
        };

        await userRepository.Add(user);

        return jwtProvider.GenerateToken(user);
    }

    public async Task<string> Login(LoginUserRequest request)
    {
        UserEntity? user = await userRepository.GetUserByEmail(request.Email);
        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        return jwtProvider.GenerateToken(user);
    }

    public async Task Update(Guid userId, UpdateUserRequest request)
    {
        var user = await userRepository.GetUserById(userId);

        if (user == null || !passwordHasher.VerifyPassword(request.oldPassword, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        var existingUser = await userRepository.GetUserByEmail(request.Email);
        if (existingUser != null && existingUser.Id != userId)
        {
            throw new EmailAlreadyExistsException();
        }

        var newPasswordHash = passwordHasher.GeneratePasswordHash(request.newPassword);

        await userRepository.Update(
            userId,
            request.Name,
            request.Email,
            newPasswordHash);
    }

    public async Task Delete(Guid userId)
    {
        await userRepository.Delete(userId);
    }
}
