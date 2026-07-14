using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectNovoLoteResolver : ITransientService
{
    Task<(int NovoIndice, int NovoLote)> ResolverAsync(
        int codigoOrganismo,
        int tipoLote,
        int tipoServico,
        int mes,
        int ano,
        CancellationToken cancellationToken = default);
}
