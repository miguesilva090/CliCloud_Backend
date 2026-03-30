using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.Filters;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService
{
  public interface ISeguradoraService : ITransientService
  {
    Task<Response<IEnumerable<SeguradoraDTO>>> GetSeguradoraAsync(string keyword = "");
    Task<Response<IEnumerable<SeguradoraLightDTO>>> GetSeguradoraLightAsync(string keyword = "");
    Task<PaginatedResponse<SeguradoraTableDTO>> GetSeguradoraPaginatedAsync(SeguradoraTableFilter filter);
    Task<Response<IEnumerable<SeguradoraTableDTO>>> GetAllSeguradoraAsync(SeguradoraAllFilter filter);
    Task<Response<SeguradoraDTO>> GetSeguradoraAsync(Guid id);
    Task<Response<Guid>> CreateSeguradoraAsync(CreateSeguradoraRequest request);
    Task<Response<Guid>> UpdateSeguradoraAsync(UpdateSeguradoraRequest request, Guid id);
    Task<Response<Guid>> DeleteSeguradoraAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleSeguradoraAsync(IEnumerable<Guid> ids);
  }
}
