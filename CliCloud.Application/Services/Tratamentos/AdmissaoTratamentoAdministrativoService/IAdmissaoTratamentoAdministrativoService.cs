using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService;

public interface IAdmissaoTratamentoAdministrativoService : ITransientService
{
  Task<PaginatedResponse<AdmissaoTratamentoTableDTO>> GetPaginatedAsync(
    AdmissaoTratamentoTableFilter filter
  );

  Task<Response<Guid>> UpdateSituacaoAsync(
    Guid id,
    UpdateAdmissaoTratamentoSituacaoRequest request
  );

  Task<Response<Guid>> DesmarcarAsync(
    Guid id,
    DesmarcarAdmissaoTratamentoRequest request
  );

  Task<Response<Guid>> RemoverDesmarcacaoAsync(Guid id);
}
