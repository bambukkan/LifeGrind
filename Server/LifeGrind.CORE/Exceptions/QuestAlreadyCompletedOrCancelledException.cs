namespace LifeGrind.CORE.Exceptions;

public class QuestAlreadyCompletedOrCancelledException : DomainException
{
    public QuestAlreadyCompletedOrCancelledException() : 
    base("Квест уже выполнен или отменен, так что нельзя отредактировать или удалить!") { }
}
