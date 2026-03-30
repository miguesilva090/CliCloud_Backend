using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Filters;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService
{
    public interface IGrupoViasAdministracaoService : ITransientService
    {
        Task<Response<IEnumerable<GrupoViasAdministracaoDTO>>> GetGrupoViasAdministracaoAsync(string keyword = "");
        Task<PaginatedResponse<GrupoViasAdministracaoTableDTO>> GetGrupoViasAdministracaoPaginatedAsync(GrupoViasAdministracaoTableFilter filter);
        Task<Response<GrupoViasAdministracaoDTO>> GetGrupoViasAdministracaoAsync(Guid id);
        Task<Response<Guid>> CreateGrupoViasAdministracaoAsync(CreateGrupoViasAdministracaoRequest request);
        Task<Response<Guid>> UpdateGrupoViasAdministracaoAsync(Guid id, UpdateGrupoViasAdministracaoRequest request);
        Task<Response<Guid>> DeleteGrupoViasAdministracaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleGrupoViasAdministracaoAsync(IEnumerable<Guid> ids);
    }
}
