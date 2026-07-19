namespace LifeGrind.CORE.Exceptions;

public class SkillNotExistException : DomainException
{
    public SkillNotExistException() : 
    base("Скилла не существует") { }
}
