using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Moedas.MoedaService.DTOs;
using CliCloud.Application.Services.Moedas.MoedaService.Filters;

namespace CliCloud.Application.Services.Moedas.MoedaService
{
    public interface IMoedaService : ITransientService
    {
        Task<Response<IEnumerable<MoedaDTO>>> GetMoedaAsync(string keyword = "");
        Task<Response<IEnumerable<MoedaLightDTO>>> GetMoedaLightAsync(string keyword = "");
        Task<PaginatedResponse<MoedaTableDTO>> GetMoedaPaginatedAsync(MoedaTableFilter filter);
        Task<Response<IEnumerable<MoedaTableDTO>>> GetAllMoedaAsync(MoedaAllFilter filter);
        Task<Response<MoedaDTO>> GetMoedaAsync(Guid id);
        Task<Response<Guid>> CreateMoedaAsync(CreateMoedaRequest request);
        Task<Response<Guid>> UpdateMoedaAsync(UpdateMoedaRequest request, Guid id);
        Task<Response<Guid>> DeleteMoedaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMoedaAsync(IEnumerable<Guid> ids);
    }
}
