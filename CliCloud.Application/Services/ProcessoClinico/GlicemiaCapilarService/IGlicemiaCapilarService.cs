using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.GlicemiaCapilarService.DTOs;
using CliCloud.Application.Services.GlicemiaCapilarService.Filters;

namespace CliCloud.Application.Services.GlicemiaCapilarService
{
    public interface IGlicemiaCapilarService : ITransientService
    {
        Task<Response<IEnumerable<GlicemiaCapilarDTO>>> GetGlicemiaCapilarAsync(string keyword = "");
        Task<PaginatedResponse<GlicemiaCapilarDTO>> GetGlicemiaCapilarPaginatedAsync(GlicemiaCapilarTableFilter filter);
        Task<Response<GlicemiaCapilarDTO>> GetGlicemiaCapilarAsync(Guid id);
        Task<Response<Guid>> CreateGlicemiaCapilarAsync(CreateGlicemiaCapilarRequest request);
        Task<Response<Guid>> UpdateGlicemiaCapilarAsync(UpdateGlicemiaCapilarRequest request, Guid id);
        Task<Response<Guid>> DeleteGlicemiaCapilarAsync(Guid id);

    }
}
