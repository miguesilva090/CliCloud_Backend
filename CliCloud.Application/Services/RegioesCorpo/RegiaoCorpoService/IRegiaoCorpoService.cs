using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Filters;

namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService
{
    public interface IRegiaoCorpoService : ITransientService
    {
        Task<Response<IEnumerable<RegiaoCorpoDTO>>> GetRegiaoCorpoAsync(string keyword = "");
        Task<Response<IEnumerable<RegiaoCorpoLightDTO>>> GetRegiaoCorpoLightAsync(string keyword = "");
        Task<PaginatedResponse<RegiaoCorpoTableDTO>> GetRegiaoCorpoPaginatedAsync(RegiaoCorpoTableFilter filter);
        Task<Response<IEnumerable<RegiaoCorpoTableDTO>>> GetAllRegiaoCorpoAsync(RegiaoCorpoAllFilter filter);
        Task<Response<RegiaoCorpoDTO>> GetRegiaoCorpoAsync(Guid id);
        Task<Response<RegiaoCorpoDTO>> GetRegiaoCorpoByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateRegiaoCorpoAsync(CreateRegiaoCorpoRequest request);
        Task<Response<Guid>> UpdateRegiaoCorpoAsync(UpdateRegiaoCorpoRequest request, Guid id);
        Task<Response<Guid>> DeleteRegiaoCorpoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleRegiaoCorpoAsync(IEnumerable<Guid> ids);
    }
}
