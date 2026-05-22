using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;

namespace CliCloud.Infrastructure.Persistence.Consultas;

/// <summary>
/// Placeholder: BD nova não tem dbo.MEDICOS nem dbo.ESPECIAL.
/// Nomes de médico resolvem-se via <see cref="MarcacoesAdministrativoService.ResolveMedicoLegadoAsync"/>.
/// </summary>
public sealed class PedidoConsultaLegacyNamesResolver : IPedidoConsultaLegacyNamesResolver
{
  public Task<string?> GetMedicoNomeAsync(string? codigoMedico, int? filtroLegado) =>
    Task.FromResult<string?>(null);

  public Task<string?> GetEspecialidadeNomeAsync(int codigoEspecialidade) =>
    Task.FromResult<string?>(null);

  public Task<IReadOnlyDictionary<int, string>> GetEspecialidadeNomesAsync(
    IEnumerable<int> codigosEspecialidade
  ) => Task.FromResult<IReadOnlyDictionary<int, string>>(new Dictionary<int, string>());
}
