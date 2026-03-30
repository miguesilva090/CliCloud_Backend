using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.Filters;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService
{
    public interface IGrupoSanguineoService : ITransientService
    {
        Task<Response<IEnumerable<GrupoSanguineoDTO>>> GetGrupoSanguineoAsync(string keyword = "");
        Task<Response<IEnumerable<GrupoSanguineoLightDTO>>> GetGrupoSanguineoLightAsync(string keyword = "");
        Task<PaginatedResponse<GrupoSanguineoTableDTO>> GetGrupoSanguineoPaginatedAsync(GrupoSanguineoTableFilter filter);
        Task<Response<IEnumerable<GrupoSanguineoTableDTO>>> GetAllGrupoSanguineoAsync(GrupoSanguineoAllFilter filter);
        Task<Response<GrupoSanguineoDTO>> GetGrupoSanguineoAsync(Guid id);
        Task<Response<Guid>> CreateGrupoSanguineoAsync(CreateGrupoSanguineoRequest request);
        Task<Response<Guid>> UpdateGrupoSanguineoAsync(UpdateGrupoSanguineoRequest request, Guid id);
        Task<Response<Guid>> DeleteGrupoSanguineoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleGrupoSanguineoAsync(IEnumerable<Guid> ids);
    }
}
