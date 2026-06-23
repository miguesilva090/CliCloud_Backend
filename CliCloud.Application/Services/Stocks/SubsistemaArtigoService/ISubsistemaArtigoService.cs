using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Filters;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService;

public interface ISubsistemaArtigoService : ITransientService
{
    Task<PaginatedResponse<SubsistemaArtigoTableDTO>> GetPaginatedAsync(SubsistemaArtigoTableFilter filter);
    Task<Response<SubsistemaArtigoDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateSubsistemaArtigoRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateSubsistemaArtigoRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}