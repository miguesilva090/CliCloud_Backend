using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.Filters;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService
{
    public interface IPrioridadeService : ITransientService
    {
        Task<Response<IEnumerable<PrioridadeDTO>>> GetPrioridadeAsync(string keyword = "");
        Task<Response<IEnumerable<PrioridadeLightDTO>>> GetPrioridadeLightAsync(string keyword = "");
        Task<PaginatedResponse<PrioridadeTableDTO>> GetPrioridadePaginatedAsync(PrioridadeTableFilter filter);
        Task<Response<IEnumerable<PrioridadeTableDTO>>> GetAllPrioridadeAsync(PrioridadeAllFilter filter);
        Task<Response<PrioridadeDTO>> GetPrioridadeAsync(Guid id);
        Task<Response<Guid>> CreatePrioridadeAsync(CreatePrioridadeRequest request);
        Task<Response<Guid>> UpdatePrioridadeAsync(UpdatePrioridadeRequest request, Guid id);
        Task<Response<Guid>> DeletePrioridadeAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultiplePrioridadeAsync(IEnumerable<Guid> ids);
    }
}
