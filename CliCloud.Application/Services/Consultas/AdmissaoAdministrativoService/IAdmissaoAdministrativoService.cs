using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

public interface IAdmissaoAdministrativoService : ITransientService
{
  Task<PaginatedResponse<AdmissaoTableDTO>> GetPaginatedAsync(AdmissaoTableFilter filter);
  Task<Response<AdmissaoDTO>> GetByIdAsync(Guid id);
  Task<Response<AdmissaoDTO?>> GetByConsultaMarcacaoIdAsync(Guid consultaMarcacaoId);
  Task<Response<Guid>> CreateAsync(CreateAdmissaoRequest request);
  Task<Response<Guid>> UpdateAsync(Guid id, UpdateAdmissaoRequest request);
  Task<Response<Guid>> DeleteAsync(Guid id);
  Task<Response<Guid>> ConfirmarAsync(Guid id, bool confirmado);
  Task<Response<Guid>> SetConfirmaConsultaAsync(Guid id, bool confirmaConsulta);
  Task<Response<Guid>> SetEmTratamentoAsync(Guid id, bool emTratamento);
  Task<Response<Guid>> SetEfetuadoAsync(Guid id, bool efetuado);
  Task<Response<Guid>> DesmarcarAsync(Guid id, DesmarcarAdmissaoRequest request);
  Task<Response<PromoverAdmissaoResultDTO>> PromoverParaConsultaAsync(Guid id);
  Task<Response<FechoDiarioResultDTO>> PromoverLoteAsync(PromoverAdmissaoLoteRequest request);
  Task<Response<AdmissaoObservacoesDTO>> GetObservacoesAsync(Guid id);
  Task<Response<Guid>> AppendObservacaoAsync(Guid id, AppendAdmissaoObservacaoRequest request);

}