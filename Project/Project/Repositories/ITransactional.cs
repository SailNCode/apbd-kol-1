namespace Tutorial9.Repositories;

public interface ITransactional
{
    Task StartTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}