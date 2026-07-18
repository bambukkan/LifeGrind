
namespace LifeGrind.CORE.Exceptions;

public class QuestHasAlreadyCompletedException : DomainException
{
    public QuestHasAlreadyCompletedException() :
     base("Квест уже выполнен!") { }
}
