using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Tutorial9.Repositories;

public class TransactionalRepository
{
    protected readonly IConfiguration _configuration;
    protected SqlConnection TransConnection { get; set; }
    protected DbTransaction Transaction { get; set; }

    public TransactionalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task StartTransactionAsync()
    {
        TransConnection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await TransConnection.OpenAsync();

        Transaction = await TransConnection.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (Transaction != null)
        {
            await Transaction.CommitAsync();
            await DisposeTransactionResourcesAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (Transaction != null)
        {
            await Transaction.RollbackAsync();
            await DisposeTransactionResourcesAsync();
        }
    }

    private async Task DisposeTransactionResourcesAsync()
    {
        await TransConnection.CloseAsync();
        await TransConnection.DisposeAsync();

        Transaction.Dispose();

        TransConnection = null;
        Transaction = null;
    }
}