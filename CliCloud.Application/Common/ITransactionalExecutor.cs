using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Common;

public interface ITransactionalExecutor : IScopedService
{
  Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default);
  Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken = default);
}
