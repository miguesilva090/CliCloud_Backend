using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;

public interface IPedidoConsultaLegacyNamesResolver : ITransientService
{
  Task<string?> GetMedicoNomeAsync(string? codigoMedico, int? filtroLegado);
  Task<string?> GetEspecialidadeNomeAsync(int codigoEspecialidade);
  Task<IReadOnlyDictionary<int, string>> GetEspecialidadeNomesAsync(
    IEnumerable<int> codigosEspecialidade
  );
}
