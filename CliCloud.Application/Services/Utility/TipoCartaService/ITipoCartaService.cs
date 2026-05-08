using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.TipoCartaService.DTOs;
using CliCloud.Application.Services.Utility.TipoCartaService.Filters;

namespace CliCloud.Application.Services.Utility.TipoCartaService
{
  public interface ITipoCartaService : ITransientService
  {
    Task<Response<IEnumerable<TipoCartaDTO>>> GetAllAsync(string keyword = "");
    Task<PaginatedResponse<TipoCartaTableDTO>> GetPaginatedAsync(TipoCartaTableFilter filter);
    Task<Response<TipoCartaDTO>> GetByIdAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateTipoCartaRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateTipoCartaRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
  }
}
