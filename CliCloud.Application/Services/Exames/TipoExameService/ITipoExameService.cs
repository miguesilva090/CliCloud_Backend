using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Exames.TipoExameService.DTOs;
using CliCloud.Application.Services.Exames.TipoExameService.Filters;

namespace CliCloud.Application.Services.Exames.TipoExameService
{
    public interface ITipoExameService : ITransientService
    {
        Task<Response<IEnumerable<TipoExameDTO>>> GetTipoExameAsync(string keyword = "");
        Task<Response<IEnumerable<TipoExameLightDTO>>> GetTipoExameLightAsync(string keyword = "");
        Task<PaginatedResponse<TipoExameTableDTO>> GetTipoExamePaginatedAsync(TipoExameTableFilter filter);
        Task<Response<IEnumerable<TipoExameTableDTO>>> GetAllTipoExameAsync(TipoExameAllFilter? filter);
        Task<Response<TipoExameDTO>> GetTipoExameAsync(Guid id);
        Task<Response<Guid>> CreateTipoExameAsync(CreateTipoExameRequest request);
        Task<Response<Guid>> UpdateTipoExameAsync(UpdateTipoExameRequest request, Guid id);
        Task<Response<Guid>> DeleteTipoExameAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoExameAsync(IEnumerable<Guid> ids);
    }
}
