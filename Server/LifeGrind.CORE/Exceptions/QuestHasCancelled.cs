

namespace LifeGrind.CORE.Exceptions;

public class QuestHasCancelled : DomainException
{
    public QuestHasCancelled() :
     base("Квест отменен, награда не начислится") { }
}
