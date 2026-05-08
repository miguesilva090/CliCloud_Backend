using System.Collections.Generic;
using System.Threading.Tasks;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.Filters;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService
{
  public interface IMotivoConsultaService : ITransientService
  {
    Task<Response<IEnumerable<MotivoConsultaDTO>>> GetAllAsync();
    Task<PaginatedResponse<MotivoConsultaTableDTO>> GetPaginatedAsync(MotivoConsultaTableFilter filter);
    Task<Response<MotivoConsultaDTO>> GetByIdAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateMotivoConsultaRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateMotivoConsultaRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
  }
}
