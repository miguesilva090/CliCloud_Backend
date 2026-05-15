using CliCloud.Application.Common;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService;

public interface IHistoricoConsultasAdministrativoService : ITransientService
{
  Task<PaginatedResponse<HistoricoConsultaAdministrativoRowDTO>> GetPaginatedAsync(
    HistoricoConsultaAdministrativoTableFilter filter
  );
}
