namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public interface ICredenciaisSnsAgregadoDeleteExecutor
{
    Task ExecutarAsync(IList<int> indices, CancellationToken cancellationToken = default);
}
