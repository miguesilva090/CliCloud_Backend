using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeService.Filters;

namespace CliCloud.Application.Services.Utility.EntidadeService
{
    public interface IEntidadeService : ITransientService
    {
        Task<Response<IEnumerable<EntidadeDTO>>> GetEntidadeAsync(string keyword = "");
        Task<Response<IEnumerable<EntidadeLightDTO>>> GetEntidadeLightAsync(string keyword = "");
        Task<PaginatedResponse<EntidadeTableDTO>> GetEntidadePaginatedAsync(EntidadeTableFilter filter);
        Task<Response<IEnumerable<EntidadeTableDTO>>> GetAllEntidadeAsync(EntidadeAllFilter filter);
        Task<Response<EntidadeDTO>> GetEntidadeAsync(Guid id);
        Task<Response<EntidadeDTO>> GetEntidadeByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<EntidadeDTO>>> GetEntidadeByNameAsync(string nome);
        Task<Response<Guid>> CreateEntidadeAsync(CreateEntidadeRequest request);
        Task<Response<Guid>> UpdateEntidadeAsync(UpdateEntidadeRequest request, Guid id);
        Task<Response<Guid>> DeleteEntidadeAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEntidadeAsync(IEnumerable<Guid> ids);

    }
}
