using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService
{
  public interface IServicoSessaoService : ITransientService
  {
    Task<Response<IEnumerable<ServicoSessaoDTO>>> GetServicoSessaoAsync(string keyword = "");
    Task<Response<IEnumerable<ServicoSessaoLightDTO>>> GetServicoSessaoLightAsync(string keyword = "");
    Task<PaginatedResponse<ServicoSessaoTableDTO>> GetServicoSessaoPaginatedAsync(ServicoSessaoTableFilter filter);
    Task<Response<IEnumerable<ServicoSessaoTableDTO>>> GetAllServicoSessaoAsync(ServicoSessaoAllFilter? filter);
    Task<Response<ServicoSessaoDTO>> GetServicoSessaoAsync(Guid id);
    Task<Response<Guid>> CreateServicoSessaoAsync(CreateServicoSessaoRequest request);
    Task<Response<Guid>> UpdateServicoSessaoAsync(UpdateServicoSessaoRequest request, Guid id);
    Task<Response<Guid>> DeleteServicoSessaoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoSessaoAsync(IEnumerable<Guid> ids);
  }
}

