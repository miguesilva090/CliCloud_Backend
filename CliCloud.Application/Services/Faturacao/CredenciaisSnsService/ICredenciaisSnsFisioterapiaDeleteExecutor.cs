using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public interface ICredenciaisSnsFisioterapiaDeleteExecutor : ITransientService
{
    Task ExecutarAsync(IList<int> indices, CancellationToken cancellationToken = default);
}