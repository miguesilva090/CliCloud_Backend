using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Atestados.AtestadoService.DTOs;
using CliCloud.Application.Services.Atestados.AtestadoService.Filters;

namespace CliCloud.Application.Services.Atestados.AtestadoService
{
  public interface IAtestadoService : ITransientService
  {
    Task<PaginatedResponse<AtestadoTableDTO>> GetAtestadoPaginatedAsync(AtestadoTableFilter filter);
    Task<Response<IEnumerable<AtestadoTableDTO>>> GetAllAtestadoAsync(AtestadoAllFilter filter);
    Task<Response<AtestadoDTO>> GetAtestadoAsync(Guid id);
    Task<Response<Guid>> CreateAtestadoAsync(CreateAtestadoRequest request);
    Task<Response<Guid>> DeleteAtestadoAsync(Guid id);
  }
}
