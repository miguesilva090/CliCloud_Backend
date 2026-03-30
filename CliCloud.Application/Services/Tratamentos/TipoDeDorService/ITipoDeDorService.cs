using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs;
using CliCloud.Application.Services.Tratamentos.TipoDeDorService.Filters;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService
{
    public interface ITipoDeDorService : ITransientService
    {
        Task<Response<IEnumerable<TipoDeDorDTO>>> GetTipoDeDorAsync(string keyword = "");
        Task<Response<IEnumerable<TipoDeDorLightDTO>>> GetTipoDeDorLightAsync(string keyword = "");
        Task<PaginatedResponse<TipoDeDorTableDTO>> GetTipoDeDorPaginatedAsync(TipoDeDorTableFilter filter);
        Task<Response<IEnumerable<TipoDeDorTableDTO>>> GetAllTipoDeDorAsync(TipoDeDorAllFilter filter);
        Task<Response<TipoDeDorDTO>> GetTipoDeDorAsync(Guid id);
        Task<Response<TipoDeDorDTO>> GetTipoDeDorByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateTipoDeDorAsync(CreateTipoDeDorRequest request);
        Task<Response<Guid>> UpdateTipoDeDorAsync(UpdateTipoDeDorRequest request, Guid id);
        Task<Response<Guid>> DeleteTipoDeDorAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoDeDorAsync(IEnumerable<Guid> ids);
    }
}
