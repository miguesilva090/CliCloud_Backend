using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.Filters;

namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService
{
    public interface IMotivoAltaService : ITransientService
    {
        Task<Response<IEnumerable<MotivoAltaDTO>>> GetMotivoAltaAsync(string keyword = "");
        Task<Response<IEnumerable<MotivoAltaLightDTO>>> GetMotivoAltaLightAsync(string keyword = "");
        Task<PaginatedResponse<MotivoAltaTableDTO>> GetMotivoAltaPaginatedAsync(MotivoAltaTableFilter filter);
        Task<Response<IEnumerable<MotivoAltaTableDTO>>> GetAllMotivoAltaAsync(MotivoAltaAllFilter filter);
        Task<Response<MotivoAltaDTO>> GetMotivoAltaAsync(Guid id);
        Task<Response<MotivoAltaDTO>> GetMotivoAltaByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateMotivoAltaAsync(CreateMotivoAltaRequest request);
        Task<Response<Guid>> UpdateMotivoAltaAsync(UpdateMotivoAltaRequest request, Guid id);
        Task<Response<Guid>> DeleteMotivoAltaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMotivoAltaAsync(IEnumerable<Guid> ids);
    }
}
