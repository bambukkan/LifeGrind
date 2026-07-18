namespace LifeGrind.CORE.Exceptions;

public class QuestNotExistException : DomainException
{
    public QuestNotExistException() : 
    base("Квеста не существует") { }
}
