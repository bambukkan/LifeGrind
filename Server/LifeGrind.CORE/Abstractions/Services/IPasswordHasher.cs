public interface IPasswordHasher
{
    string GeneratePasswordHash(string password);
    bool VerifyPassword(string password,string hashedPassword);
}