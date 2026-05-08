using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.SalaService.DTOs;
using CliCloud.Application.Services.Consultas.SalaService.Filters;

namespace CliCloud.Application.Services.Consultas.SalaService
{
  public interface ISalaService : ITransientService
  {
    Task<Response<IEnumerable<SalaDTO>>> GetAllAsync(string keyword = "");
    Task<PaginatedResponse<SalaTableDTO>> GetPaginatedAsync(SalaTableFilter filter);
    Task<Response<SalaDTO>> GetByIdAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateSalaRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateSalaRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
  }
}
