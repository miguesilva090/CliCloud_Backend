using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.TipoServicoService;

public interface ITipoServicoCodigoLookup : ITransientService
{
  /// <summary>
  /// Nomes por Codigo em Servicos.TipoServico.
  /// Preferência: linha com Filtro = clínica (Clinica.Cid); senão primeira por Filtro.
  /// </summary>
  Task<IReadOnlyDictionary<int, string>> ObterNomesPorCodigoAsync(
    IReadOnlyCollection<int> codigos,
    int? filtroClinica = null,
    CancellationToken cancellationToken = default);
}
