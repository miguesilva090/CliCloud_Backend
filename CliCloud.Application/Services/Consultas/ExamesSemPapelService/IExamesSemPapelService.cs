using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;

namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService;

public interface IExamesSemPapelService : ITransientService
{
  Task<Response<ExamesSemPapelContextoDTO>> ObterContextoAsync(Guid clinicaId);
  Task<PaginatedResponse<ExameSemPapelTabelaDTO>> ObterTabelaAsync(Guid clinicaId, ExameSemPapelFiltroRequest request);
  Task<Response<bool>> GuardarAssinaturaSessaoAsync(ExameSemPapelAssinaturaSessaoRequest request);
  Task<Response<bool>> LimparAssinaturaSessaoAsync();
  Task<Response<ExameSemPapelLoteResultadoDTO>> AssinarLoteAsync(Guid clinicaId, ExameSemPapelLoteRequest request);
  Task<Response<ExameSemPapelLoteResultadoDTO>> ComunicarLoteAsync(Guid clinicaId, ExameSemPapelLoteRequest request);
}
