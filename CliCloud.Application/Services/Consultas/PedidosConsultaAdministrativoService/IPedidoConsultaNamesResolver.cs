using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;

public interface IPedidoConsultaNamesResolver : ITransientService
{
  Task<string?> GetMedicoNomeAsync(string? codigoMedico, int? filtroClinica);
  Task<string?> GetEspecialidadeNomeAsync(int codigoEspecialidade);
  Task<IReadOnlyDictionary<int, string>> GetEspecialidadeNomesAsync(
    IEnumerable<int> codigosEspecialidade
  );
}
