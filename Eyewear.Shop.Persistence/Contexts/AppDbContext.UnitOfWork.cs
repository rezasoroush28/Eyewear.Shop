using Microsoft.EntityFrameworkCore.Storage;

namespace Eyewear.Shop.Persistence.Contexts;

public partial class AppDbContext : IUnitOfWork
{
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await base.Database.BeginTransactionAsync();
    }

    public async Task RollbackTransaction(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
    }
}

