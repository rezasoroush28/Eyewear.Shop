using Microsoft.EntityFrameworkCore.Storage;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task RollbackTransaction(IDbContextTransaction transaction);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}