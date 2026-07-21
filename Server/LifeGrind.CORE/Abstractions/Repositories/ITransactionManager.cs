public interface ITransactionManager
{
    Task ExecuteAsync(Func<Task> action);

}