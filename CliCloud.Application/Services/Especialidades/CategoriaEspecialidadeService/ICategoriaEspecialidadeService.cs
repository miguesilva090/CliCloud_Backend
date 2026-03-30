using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Filters;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService
{
    public interface ICategoriaEspecialidadeService : ITransientService
    {
        Task<Response<IEnumerable<CategoriaEspecialidadeDTO>>> GetCategoriaEspecialidadeAsync(string keyword = "");
        Task<Response<IEnumerable<CategoriaEspecialidadeLightDTO>>> GetCategoriaEspecialidadeLightAsync(string keyword = "");
        Task<PaginatedResponse<CategoriaEspecialidadeTableDTO>> GetCategoriaEspecialidadePaginatedAsync(CategoriaEspecialidadeTableFilter filter);
        Task<Response<IEnumerable<CategoriaEspecialidadeTableDTO>>> GetAllCategoriaEspecialidadeAsync(CategoriaEspecialidadeAllFilter filter);
        Task<Response<CategoriaEspecialidadeDTO>> GetCategoriaEspecialidadeAsync(Guid id);
        Task<Response<Guid>> CreateCategoriaEspecialidadeAsync(CreateCategoriaEspecialidadeRequest request);
        Task<Response<Guid>> UpdateCategoriaEspecialidadeAsync(UpdateCategoriaEspecialidadeRequest request, Guid id);
        Task<Response<Guid>> DeleteCategoriaEspecialidadeAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleCategoriaEspecialidadeAsync(IEnumerable<Guid> ids);
    }
}
