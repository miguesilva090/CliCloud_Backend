using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.ServicoService.DTOs;
using CliCloud.Application.Services.Servicos.ServicoService.Filters;

namespace CliCloud.Application.Services.Servicos.ServicoService
{
  public interface IServicoService : ITransientService
  {
    Task<Response<IEnumerable<ServicoDTO>>> GetServicoAsync(string keyword = "");
    Task<Response<IEnumerable<ServicoLightDTO>>> GetServicoLightAsync(string keyword = "");
    Task<PaginatedResponse<ServicoTableDTO>> GetServicoPaginatedAsync(ServicoTableFilter filter);
    Task<Response<IEnumerable<ServicoTableDTO>>> GetAllServicoAsync(ServicoAllFilter? filter);
    Task<Response<ServicoDTO>> GetServicoAsync(Guid id);
    Task<Response<Guid>> CreateServicoAsync(CreateServicoRequest request);
    Task<Response<Guid>> UpdateServicoAsync(UpdateServicoRequest request, Guid id);
    Task<Response<Guid>> DeleteServicoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoAsync(IEnumerable<Guid> ids);
  }
}

