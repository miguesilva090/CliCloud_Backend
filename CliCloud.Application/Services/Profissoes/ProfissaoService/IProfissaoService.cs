using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs;
using CliCloud.Application.Services.Profissoes.ProfissaoService.Filters;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService
{
    public interface IProfissaoService : ITransientService
    {
        Task<Response<IEnumerable<ProfissaoDTO>>> GetProfissaoAsync(string keyword = "");
        Task<Response<IEnumerable<ProfissaoLightDTO>>> GetProfissaoLightAsync(string keyword = "");
        Task<PaginatedResponse<ProfissaoTableDTO>> GetProfissaoPaginatedAsync(ProfissaoTableFilter filter);
        Task<Response<IEnumerable<ProfissaoTableDTO>>> GetAllProfissaoAsync(ProfissaoAllFilter filter);
        Task<Response<ProfissaoDTO>> GetProfissaoAsync(Guid id);
        Task<Response<Guid>> CreateProfissaoAsync(CreateProfissaoRequest request);
        Task<Response<Guid>> UpdateProfissaoAsync(UpdateProfissaoRequest request, Guid id);
        Task<Response<Guid>> DeleteProfissaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleProfissaoAsync(IEnumerable<Guid> ids);
    }
}
