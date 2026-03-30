using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.GrauAlergiaService.DTOs;
using CliCloud.Application.Services.GrauAlergiaService.Filters;

namespace CliCloud.Application.Services.GrauAlergiaService
{
    public interface IGrauAlergiaService : ITransientService
    {
        Task<Response<IEnumerable<GrauAlergiaDTO>>> GetGrauAlergiaAsync(string keyword = "");
        Task<Response<IEnumerable<GrauAlergiaLightDTO>>> GetGrauAlergiaLightAsync(string keyword = "");
        Task<PaginatedResponse<GrauAlergiaTableDTO>> GetGrauAlergiaPaginatedAsync(GrauAlergiaTableFilter filter);
        Task<Response<IEnumerable<GrauAlergiaTableDTO>>> GetAllGrauAlergiaAsync(GrauAlergiaAllFilter filter);
        Task<Response<GrauAlergiaDTO>> GetGrauAlergiaAsync(Guid id);
        Task<Response<Guid>> CreateGrauAlergiaAsync(CreateGrauAlergiaRequest request);
        Task<Response<Guid>> UpdateGrauAlergiaAsync(UpdateGrauAlergiaRequest request, Guid id);
        Task<Response<Guid>> DeleteGrauAlergiaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleGrauAlergiaAsync(IEnumerable<Guid> ids);
    }
}
