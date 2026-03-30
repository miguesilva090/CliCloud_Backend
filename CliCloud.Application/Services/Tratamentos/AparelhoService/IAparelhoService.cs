using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.AparelhoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService
{
  public interface IAparelhoService : ITransientService
  {
    Task<Response<IEnumerable<AparelhoDTO>>> GetAparelhoAsync(string keyword = "");
    Task<Response<IEnumerable<AparelhoLightDTO>>> GetAparelhoLightAsync(string keyword = "");
    Task<PaginatedResponse<AparelhoTableDTO>> GetAparelhoPaginatedAsync(AparelhoTableFilter filter);
    Task<Response<IEnumerable<AparelhoTableDTO>>> GetAllAparelhoAsync(AparelhoAllFilter filter);
    Task<Response<AparelhoDTO>> GetAparelhoAsync(Guid id);
    Task<Response<AparelhoDTO>> GetAparelhoByCodigoSerieAsync(string codigoSerie);
    Task<Response<Guid>> CreateAparelhoAsync(CreateAparelhoRequest request);
    Task<Response<Guid>> UpdateAparelhoAsync(UpdateAparelhoRequest request, Guid id);
    Task<Response<Guid>> DeleteAparelhoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAparelhoAsync(IEnumerable<Guid> ids);
  }
}
