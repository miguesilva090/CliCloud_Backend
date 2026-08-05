using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService;

public interface IPlanningTratamentoAdministrativoService : ITransientService
{
    Task<Response<PlanningSessoesResponse>> GetSessoesAsync(PlanningSessoesRequest request);
}