using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService;

public interface IHistoricoConsultasAdministrativoService : ITransientService
{
  Task<PaginatedResponse<HistoricoConsultaAdministrativoRowDTO>> GetPaginatedAsync(
    HistoricoConsultaAdministrativoTableFilter filter
  );
  Task<Response<AdmissaoDTO>> GetConsultaHistoricoForEditAsync(Guid consultaId);
  Task<Response<Guid>> UpdateConsultaHistoricoAsync(Guid consultaId, UpdateConsultaHistoricoRequest request);
}
