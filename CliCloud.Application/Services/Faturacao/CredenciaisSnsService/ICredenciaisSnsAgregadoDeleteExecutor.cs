using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public interface ICredenciaisSnsAgregadoDeleteExecutor : ITransientService
{
    Task ExecutarAsync(IList<int> indices, CancellationToken cancellationToken = default);
}
