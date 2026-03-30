using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.ReciboService.DTOs;
using CliCloud.Application.Services.Documentos.ReciboService.Filters;

namespace CliCloud.Application.Services.Documentos.ReciboService
{
  /// <summary>
  /// Serviço de leitura de Recibos (Documento : Recibo).
  /// Create/Update/Delete de recibos ficam no fluxo de DocumentoService / emissão de documentos.
  /// </summary>
  public interface IReciboService : ITransientService
  {
    Task<Response<IEnumerable<ReciboDTO>>> GetReciboAsync(string keyword = "");
    Task<Response<IEnumerable<ReciboLightDTO>>> GetReciboLightAsync(string keyword = "");
    Task<PaginatedResponse<ReciboTableDTO>> GetReciboPaginatedAsync(ReciboTableFilter filter);
    Task<Response<IEnumerable<ReciboTableDTO>>> GetAllReciboAsync(ReciboAllFilter filter);
    Task<Response<ReciboDTO>> GetReciboAsync(Guid id);
  }
}
