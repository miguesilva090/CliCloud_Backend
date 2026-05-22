using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;

public interface IPedidosConsultaAdministrativoService : ITransientService
{
  Task<PaginatedResponse<PedidoConsultaTableDTO>> GetPaginatedAsync(PedidoConsultaTableFilter filter);
  Task<Response<PedidoConsultaDTO>> GetByIdAsync(int codigo);
  Task<Response<int>> SetRecusadoAsync(int codigo, SetPedidoConsultaRecusadoRequest request);
  Task<Response<int>> DeleteAsync(int codigo);
  Task<Response<int[]>> DeleteMultipleAsync(IEnumerable<int> codigos);
  Task<Response<PedidoConsultaFicheiroDTO>> DownloadFicheiroAsync(int codigo);
  Task<Response<PedidoConsultaUtentesCandidatosDTO>> PesquisarUtentesAsync(int codigo);
  Task<Response<Guid>> CriarUtenteFromPedidoAsync(int codigo, bool forcar);
  Task<Response<GuardarPedidoConsultaMarcacaoResultDTO>> GuardarMarcacaoAsync(
    int codigo,
    GuardarPedidoConsultaMarcacaoRequest request
  );
  Task<Response<int>> EnviarEmailAsync(int codigo, int tipo);
  Task<Response<int>> EnviarSmsAsync(int codigo, int tipo);
}
