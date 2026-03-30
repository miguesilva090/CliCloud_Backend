using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs;
using CliCloud.Application.Services.Especialidades.EspecialidadeService.Filters;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService
{
    public interface IEspecialidadeService : ITransientService
    {
        Task<Response<IEnumerable<EspecialidadeDTO>>> GetEspecialidadeAsync(string keyword = "");
        Task<Response<IEnumerable<EspecialidadeLightDTO>>> GetEspecialidadeLightAsync(string keyword = "");
        Task<PaginatedResponse<EspecialidadeTableDTO>> GetEspecialidadePaginatedAsync(EspecialidadeTableFilter filter);
        Task<Response<IEnumerable<EspecialidadeTableDTO>>> GetAllEspecialidadeAsync(EspecialidadeAllFilter filter);
        Task<Response<EspecialidadeDTO>> GetEspecialidadeAsync(Guid id);
        Task<Response<IEnumerable<EspecialidadeDTO>>> GetEspecialidadeByNameAsync(string nome);
        Task<Response<Guid>> CreateEspecialidadeAsync(CreateEspecialidadeRequest request);
        Task<Response<Guid>> UpdateEspecialidadeAsync(UpdateEspecialidadeRequest request, Guid id);
        Task<Response<Guid>> DeleteEspecialidadeAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEspecialidadeAsync(IEnumerable<Guid> ids);
    }
}
