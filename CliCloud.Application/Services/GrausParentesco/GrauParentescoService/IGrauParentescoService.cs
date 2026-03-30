using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs;
using CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Filters;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService
{
    public interface IGrauParentescoService : ITransientService
    {
        Task<Response<IEnumerable<GrauParentescoDTO>>> GetGrauParentescoAsync(string keyword = "");
        Task<Response<IEnumerable<GrauParentescoLightDTO>>> GetGrauParentescoLightAsync(string keyword = "");
        Task<PaginatedResponse<GrauParentescoTableDTO>> GetGrauParentescoPaginatedAsync(GrauParentescoTableFilter filter);
        Task<Response<IEnumerable<GrauParentescoTableDTO>>> GetAllGrauParentescoAsync(GrauParentescoAllFilter filter);
        Task<Response<GrauParentescoDTO>> GetGrauParentescoAsync(Guid id);
        Task<Response<Guid>> CreateGrauParentescoAsync(CreateGrauParentescoRequest request);
        Task<Response<Guid>> UpdateGrauParentescoAsync(UpdateGrauParentescoRequest request, Guid id);
        Task<Response<Guid>> DeleteGrauParentescoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleGrauParentescoAsync(IEnumerable<Guid> ids);
    }
}
