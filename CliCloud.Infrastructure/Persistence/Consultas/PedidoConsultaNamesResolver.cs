using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;

namespace CliCloud.Infrastructure.Persistence.Consultas;

/// <summary>
/// Placeholder: o modelo novo não mantém catálogos antigos de nomes neste fluxo.
/// A resolução nominal é feita em serviços administrativos próprios.
/// </summary>
public sealed class PedidoConsultaNamesResolver : IPedidoConsultaNamesResolver
{
  public Task<string?> GetMedicoNomeAsync(string? codigoMedico, int? filtroClinica) =>
    Task.FromResult<string?>(null);

  public Task<string?> GetEspecialidadeNomeAsync(int codigoEspecialidade) =>
    Task.FromResult<string?>(null);

  public Task<IReadOnlyDictionary<int, string>> GetEspecialidadeNomesAsync(
    IEnumerable<int> codigosEspecialidade
  ) => Task.FromResult<IReadOnlyDictionary<int, string>>(new Dictionary<int, string>());
}
