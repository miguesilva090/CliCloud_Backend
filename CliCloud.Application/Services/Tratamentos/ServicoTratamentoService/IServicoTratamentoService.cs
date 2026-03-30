using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService
{
  public interface IServicoTratamentoService : ITransientService
  {
    Task<Response<IEnumerable<ServicoTratamentoDTO>>> GetServicoTratamentoAsync(string keyword = "");
    Task<Response<IEnumerable<ServicoTratamentoLightDTO>>> GetServicoTratamentoLightAsync(string keyword = "");
    Task<PaginatedResponse<ServicoTratamentoTableDTO>> GetServicoTratamentoPaginatedAsync(ServicoTratamentoTableFilter filter);
    Task<Response<IEnumerable<ServicoTratamentoTableDTO>>> GetAllServicoTratamentoAsync(ServicoTratamentoAllFilter? filter);
    Task<Response<ServicoTratamentoDTO>> GetServicoTratamentoAsync(Guid id);
    Task<Response<Guid>> CreateServicoTratamentoAsync(CreateServicoTratamentoRequest request);
    Task<Response<Guid>> UpdateServicoTratamentoAsync(UpdateServicoTratamentoRequest request, Guid id);
    Task<Response<Guid>> DeleteServicoTratamentoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoTratamentoAsync(IEnumerable<Guid> ids);
  }
}

