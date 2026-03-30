using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Filters;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService
{
    public interface IMotivoIsencaoService : ITransientService
    {
        Task<Response<IEnumerable<MotivoIsencaoDTO>>> GetMotivoIsencaoAsync(string keyword = "");
        Task<Response<IEnumerable<MotivoIsencaoLightDTO>>> GetMotivoIsencaoLightAsync(string keyword = "");
        Task<PaginatedResponse<MotivoIsencaoTableDTO>> GetMotivoIsencaoPaginatedAsync(MotivoIsencaoTableFilter filter);
        Task<Response<IEnumerable<MotivoIsencaoTableDTO>>> GetAllMotivoIsencaoAsync(MotivoIsencaoAllFilter? filter);
        Task<Response<MotivoIsencaoDTO>> GetMotivoIsencaoAsync(Guid id);
        Task<Response<Guid>> CreateMotivoIsencaoAsync(CreateMotivoIsencaoRequest request);
        Task<Response<Guid>> UpdateMotivoIsencaoAsync(UpdateMotivoIsencaoRequest request, Guid id);
        Task<Response<Guid>> DeleteMotivoIsencaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivoIsencaoAsync(IEnumerable<Guid> ids);
    }
}
