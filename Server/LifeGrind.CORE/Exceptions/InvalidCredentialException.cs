namespace LifeGrind.CORE.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException() : base("Пользователя не существует или неверный пароль.") { }
}
