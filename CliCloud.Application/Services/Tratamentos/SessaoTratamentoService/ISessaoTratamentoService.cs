using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService
{
  public interface ISessaoTratamentoService : ITransientService
  {
    Task<Response<IEnumerable<SessaoTratamentoDTO>>> GetSessaoTratamentoAsync(string keyword = "");
    Task<Response<IEnumerable<SessaoTratamentoLightDTO>>> GetSessaoTratamentoLightAsync(string keyword = "");
    Task<PaginatedResponse<SessaoTratamentoTableDTO>> GetSessaoTratamentoPaginatedAsync(SessaoTratamentoTableFilter filter);
    Task<Response<IEnumerable<SessaoTratamentoTableDTO>>> GetAllSessaoTratamentoAsync(SessaoTratamentoAllFilter? filter);
    Task<Response<SessaoTratamentoDTO>> GetSessaoTratamentoAsync(Guid id);
    Task<Response<Guid>> CreateSessaoTratamentoAsync(CreateSessaoTratamentoRequest request);
    Task<Response<Guid>> UpdateSessaoTratamentoAsync(UpdateSessaoTratamentoRequest request, Guid id);
    Task<Response<Guid>> DeleteSessaoTratamentoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleSessaoTratamentoAsync(IEnumerable<Guid> ids);
    Task<Response<Guid>> CompensarFaltaAsync(CompensarFaltaSessaoTratamentoRequest request);
  }
}

