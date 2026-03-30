using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.DTOs;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.Filters;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService
{
  public interface ISubsistemaServicoService : ITransientService
  {
    Task<Response<IEnumerable<SubsistemaServicoDTO>>> GetSubsistemaServicoAsync(Guid? servicoId = null);
    Task<PaginatedResponse<SubsistemaServicoDTO>> GetSubsistemaServicoPaginatedAsync(SubsistemaServicoTableFilter filter);
    Task<Response<SubsistemaServicoDTO>> GetSubsistemaServicoAsync(Guid id);
    Task<Response<Guid>> CreateSubsistemaServicoAsync(CreateSubsistemaServicoRequest request);
    Task<Response<Guid>> UpdateSubsistemaServicoAsync(UpdateSubsistemaServicoRequest request, Guid id);
    Task<Response<Guid>> DeleteSubsistemaServicoAsync(Guid id);
  }
}
