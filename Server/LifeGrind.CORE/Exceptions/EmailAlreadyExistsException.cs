
namespace LifeGrind.CORE.Exceptions;

public class EmailAlreadyExistsException : DomainException
{
    public EmailAlreadyExistsException() :
     base("Email уже существует!") { }
}
