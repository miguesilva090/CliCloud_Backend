using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs;
using CliCloud.Application.Services.Tratamentos.PatologiaService.Filters;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService
{
  public interface IPatologiaService : ITransientService
  {
    Task<Response<IEnumerable<PatologiaDTO>>> GetPatologiaAsync(string keyword = "");
    Task<Response<IEnumerable<PatologiaLightDTO>>> GetPatologiaLightAsync(string keyword = "");
    Task<PaginatedResponse<PatologiaTableDTO>> GetPatologiaPaginatedAsync(PatologiaTableFilter filter);
    Task<Response<IEnumerable<PatologiaTableDTO>>> GetAllPatologiaAsync(PatologiaAllFilter filter);
    Task<Response<PatologiaDTO>> GetPatologiaAsync(Guid id);
    Task<Response<Guid>> CreatePatologiaAsync(CreatePatologiaRequest request);
    Task<Response<Guid>> UpdatePatologiaAsync(UpdatePatologiaRequest request, Guid id);
    Task<Response<Guid>> DeletePatologiaAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultiplePatologiaAsync(IEnumerable<Guid> ids);
  }
}
