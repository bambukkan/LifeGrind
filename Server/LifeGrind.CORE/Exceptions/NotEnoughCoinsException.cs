namespace LifeGrind.CORE.Exceptions;

public class NotEnoughCoinsException : DomainException
{
    public NotEnoughCoinsException()
        : base("Недостаточно монет для получения награды.") { }
}
