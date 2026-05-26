using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService;

public interface IOrdemEntradaAdministrativoService : ITransientService
{
  Task<PaginatedResponse<OrdemEntradaTableDTO>> GetPaginatedAsync(OrdemEntradaTableFilter filter);
  Task<Response<OrdemEntradaRegistoDTO>> GetRegistoAsync(Guid id);
  Task<Response<Guid>> CreateRegistoAsync(SaveOrdemEntradaRegistoRequest request);
  Task<Response<Guid>> UpdateRegistoAsync(Guid id, SaveOrdemEntradaRegistoRequest request);
  Task<Response<Guid>> AnularAsync(Guid id, AnularOrdemEntradaRequest request);
  Task<Response<OrdemEntradaHorasDisponiveisDTO>> GetHorasDisponiveisAsync(
    OrdemEntradaHorasDisponiveisRequest request
  );
}
