using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Filters;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService
{
    public interface IHabilitacaoService : ITransientService
    {
        Task<Response<IEnumerable<HabilitacaoDTO>>> GetHabilitacaoAsync(string keyword = "");
        Task<Response<IEnumerable<HabilitacaoLightDTO>>> GetHabilitacaoLightAsync(string keyword = "");
        Task<PaginatedResponse<HabilitacaoTableDTO>> GetHabilitacaoPaginatedAsync(HabilitacaoTableFilter filter);
        Task<Response<IEnumerable<HabilitacaoTableDTO>>> GetAllHabilitacaoAsync(HabilitacaoAllFilter filter);
        Task<Response<HabilitacaoDTO>> GetHabilitacaoAsync(Guid id);
        Task<Response<Guid>> CreateHabilitacaoAsync(CreateHabilitacaoRequest request);
        Task<Response<Guid>> UpdateHabilitacaoAsync(UpdateHabilitacaoRequest request, Guid id);
        Task<Response<Guid>> DeleteHabilitacaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHabilitacaoAsync(IEnumerable<Guid> ids);
    }
}
