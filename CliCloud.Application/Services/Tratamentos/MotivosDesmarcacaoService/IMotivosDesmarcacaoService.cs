using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService
{
    public interface IMotivosDesmarcacaoService : ITransientService
    {
        Task<Response<IEnumerable<MotivosDesmarcacaoDTO>>> GetMotivosDesmarcacaoAsync(string keyword = "");
        Task<Response<IEnumerable<MotivosDesmarcacaoLightDTO>>> GetMotivosDesmarcacaoLightAsync(string keyword = "");
        Task<Response<IEnumerable<MotivosDesmarcacaoTableDTO>>> GetAllMotivosDesmarcacaoAsync(MotivosDesmarcacaoAllFilter filter);
        Task<PaginatedResponse<MotivosDesmarcacaoTableDTO>> GetMotivosDesmarcacaoPaginatedAsync(MotivosDesmarcacaoTableFilter filter);
        Task<Response<MotivosDesmarcacaoDTO>> GetMotivosDesmarcacaoAsync(Guid id);
        Task<Response<MotivosDesmarcacaoDTO>> GetMotivosDesmarcacaoByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateMotivosDesmarcacaoAsync(CreateMotivosDesmarcacaoRequest request);
        Task<Response<Guid>> UpdateMotivosDesmarcacaoAsync(UpdateMotivosDesmarcacaoRequest request, Guid id);
        Task<Response<Guid>> DeleteMotivosDesmarcacaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivosDesmarcacaoAsync(IEnumerable<Guid> ids);
    }
}
