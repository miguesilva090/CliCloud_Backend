using CliCloud.Application.Common;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Transaction;

public class TransactionalExecutor(ApplicationDbContext dbContext) : ITransactionalExecutor
{
  public async Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (dbContext.Database.CurrentTransaction != null)
    {
      await action(cancellationToken);
      return;
    }

    await using IDbContextTransaction tx = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      await action(cancellationToken);
      await tx.CommitAsync(cancellationToken);
    }
    catch
    {
      await tx.RollbackAsync(cancellationToken);
      throw;
    }
  }

  public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(action);

    if (dbContext.Database.CurrentTransaction != null)
    {
      return await action(cancellationToken);
    }

    await using IDbContextTransaction tx = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    try
    {
      T result = await action(cancellationToken);
      await tx.CommitAsync(cancellationToken);
      return result;
    }
    catch
    {
      await tx.RollbackAsync(cancellationToken);
      throw;
    }
  }
}
