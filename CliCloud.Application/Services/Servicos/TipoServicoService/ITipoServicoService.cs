using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.TipoServicoService.DTOs;
using CliCloud.Application.Services.Servicos.TipoServicoService.Filters;

namespace CliCloud.Application.Services.Servicos.TipoServicoService
{
  public interface ITipoServicoService : ITransientService
  {
    Task<Response<IEnumerable<TipoServicoDTO>>> GetTipoServicoAsync(string keyword = "");
    Task<Response<IEnumerable<TipoServicoLightDTO>>> GetTipoServicoLightAsync(string keyword = "");
    Task<PaginatedResponse<TipoServicoTableDTO>> GetTipoServicoPaginatedAsync(TipoServicoTableFilter filter);
    Task<Response<IEnumerable<TipoServicoTableDTO>>> GetAllTipoServicoAsync(TipoServicoAllFilter? filter);
    Task<Response<TipoServicoDTO>> GetTipoServicoAsync(Guid id);
    Task<Response<Guid>> CreateTipoServicoAsync(CreateTipoServicoRequest request);
    Task<Response<Guid>> UpdateTipoServicoAsync(UpdateTipoServicoRequest request, Guid id);
    Task<Response<Guid>> DeleteTipoServicoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoServicoAsync(IEnumerable<Guid> ids);
  }
}

