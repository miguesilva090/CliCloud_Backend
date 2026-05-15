using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

public interface IAdmissaoAdministrativoService : ITransientService
{
  Task<PaginatedResponse<AdmissaoTableDTO>> GetPaginatedAsync(AdmissaoTableFilter filter);
  Task<Response<AdmissaoDTO>> GetByIdAsync(Guid id);
  Task<Response<Guid>> CreateAsync(CreateAdmissaoRequest request);
  Task<Response<Guid>> UpdateAsync(Guid id, UpdateAdmissaoRequest request);
  Task<Response<Guid>> DeleteAsync(Guid id);
  Task<Response<Guid>> ConfirmarAsync(Guid id, bool confirmado);
  Task<Response<Guid>> SetEfetuadoAsync(Guid id, bool efetuado);
  Task<Response<Guid>> PromoverParaConsultaAsync(Guid id);
}
