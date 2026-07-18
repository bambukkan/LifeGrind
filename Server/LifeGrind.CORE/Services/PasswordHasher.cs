using System.CodeDom.Compiler;

public class PasswordHasher : IPasswordHasher
{
    public string GeneratePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    public bool VerifyPassword(string password,string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password,hashedPassword);
    }
}