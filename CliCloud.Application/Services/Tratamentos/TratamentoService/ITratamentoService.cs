using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService
{
  public interface ITratamentoService : ITransientService
  {
    Task<Response<IEnumerable<TratamentoDTO>>> GetTratamentoAsync(string keyword = "");
    Task<Response<IEnumerable<TratamentoLightDTO>>> GetTratamentoLightAsync(string keyword = "");
    Task<PaginatedResponse<TratamentoTableDTO>> GetTratamentoPaginatedAsync(TratamentoTableFilter filter);
    Task<Response<IEnumerable<TratamentoTableDTO>>> GetAllTratamentoAsync(TratamentoAllFilter? filter);
    Task<Response<TratamentoDTO>> GetTratamentoAsync(Guid id);
    Task<Response<Guid>> CreateTratamentoAsync(CreateTratamentoRequest request);
    Task<Response<Guid>> UpdateTratamentoAsync(UpdateTratamentoRequest request, Guid id);
    Task<Response<Guid>> DeleteTratamentoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleTratamentoAsync(IEnumerable<Guid> ids);
    Task<Response<Guid>> UpdateTratamentoAltaAsync(Guid id, bool alta);
  }
}

