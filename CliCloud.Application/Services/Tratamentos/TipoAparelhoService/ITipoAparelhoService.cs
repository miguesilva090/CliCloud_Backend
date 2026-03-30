using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService
{
  public interface ITipoAparelhoService : ITransientService
  {
    Task<Response<IEnumerable<TipoAparelhoDTO>>> GetTipoAparelhoAsync(string keyword = "");
    Task<Response<IEnumerable<TipoAparelhoLightDTO>>> GetTipoAparelhoLightAsync(string keyword = "");
    Task<PaginatedResponse<TipoAparelhoTableDTO>> GetTipoAparelhoPaginatedAsync(TipoAparelhoTableFilter filter);
    Task<Response<IEnumerable<TipoAparelhoTableDTO>>> GetAllTipoAparelhoAsync(TipoAparelhoAllFilter filter);
    Task<Response<TipoAparelhoDTO>> GetTipoAparelhoAsync(Guid id);
    Task<Response<TipoAparelhoDTO>> GetTipoAparelhoByDesignacaoAsync(string designacao);
    Task<Response<Guid>> CreateTipoAparelhoAsync(CreateTipoAparelhoRequest request);
    Task<Response<Guid>> UpdateTipoAparelhoAsync(UpdateTipoAparelhoRequest request, Guid id);
    Task<Response<Guid>> DeleteTipoAparelhoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoAparelhoAsync(IEnumerable<Guid> ids);
  }
}
