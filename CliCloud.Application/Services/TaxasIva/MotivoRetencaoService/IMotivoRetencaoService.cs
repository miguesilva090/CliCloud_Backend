using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Filters;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService
{
    public interface IMotivoRetencaoService : ITransientService
    {
        Task<Response<IEnumerable<MotivoRetencaoDTO>>> GetMotivoRetencaoAsync(string keyword = "");
        Task<Response<IEnumerable<MotivoRetencaoLightDTO>>> GetMotivoRetencaoLightAsync(string keyword = "", string? tipoImposto = null);
        Task<PaginatedResponse<MotivoRetencaoTableDTO>> GetMotivoRetencaoPaginatedAsync(MotivoRetencaoTableFilter filter);
        Task<Response<IEnumerable<MotivoRetencaoTableDTO>>> GetAllMotivoRetencaoAsync(MotivoRetencaoAllFilter? filter);
        Task<Response<MotivoRetencaoDTO>> GetMotivoRetencaoAsync(Guid id);
        Task<Response<Guid>> CreateMotivoRetencaoAsync(CreateMotivoRetencaoRequest request);
        Task<Response<Guid>> UpdateMotivoRetencaoAsync(UpdateMotivoRetencaoRequest request, Guid id);
        Task<Response<Guid>> DeleteMotivoRetencaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivoRetencaoAsync(IEnumerable<Guid> ids);
    }
}
