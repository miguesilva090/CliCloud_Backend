using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService
{
    public interface ITiposTratamentoDentarioService : ITransientService
    {
        Task<Response<IEnumerable<TiposTratamentoDentarioDTO>>> GetTiposTratamentoDentarioAsync(string keyword = "");
        Task<PaginatedResponse<TiposTratamentoDentarioDTO>> GetTiposTratamentoDentarioPaginatedAsync(TiposTratamentoDentarioTableFilter filter);
        Task<Response<TiposTratamentoDentarioDTO>> GetTiposTratamentoDentarioAsync(Guid id);
        Task<Response<Guid>> CreateTiposTratamentoDentarioAsync(CreateTiposTratamentoDentarioRequest request);
        Task<Response<Guid>> UpdateTiposTratamentoDentarioAsync(UpdateTiposTratamentoDentarioRequest request, Guid id);
        Task<Response<Guid>> DeleteTiposTratamentoDentarioAsync(Guid id);

    }
}
