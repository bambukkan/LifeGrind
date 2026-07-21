public class TransactionManager : ITransactionManager
{
    private readonly LifeGrindDbContext context;
    public TransactionManager(LifeGrindDbContext _context)
    {
        context = _context;
    }
    public async Task ExecuteAsync(Func<Task> action)
    {
         await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await action();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}