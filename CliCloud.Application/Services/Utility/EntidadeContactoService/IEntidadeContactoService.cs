using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Filters;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService
{
    public interface IEntidadeContactoService : ITransientService
    {
        Task<Response<IEnumerable<EntidadeContactoDTO>>> GetEntidadeContactoAsync(string keyword = "");
        Task<PaginatedResponse<EntidadeContactoDTO>> GetEntidadeContactoPaginatedAsync(EntidadeContactoTableFilter filter);
        Task<Response<EntidadeContactoDTO>> GetEntidadeContactoAsync(Guid id);
        Task<Response<Guid>> CreateEntidadeContactoAsync(CreateEntidadeContactoRequest request);
        Task<Response<IEnumerable<Guid>>> CreateEntidadeContactoBulkAsync(CreateEntidadeContactoBulkRequest request);
        Task<Response<Guid>> UpdateEntidadeContactoAsync(UpdateEntidadeContactoRequest request, Guid id);
        Task<Response<Guid>> DeleteEntidadeContactoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> UpdateEntidadeContactoBulkAsync(UpdateEntidadeContactoBulkRequest request);
        Task<Response<IEnumerable<Guid>>> UpsertEntidadeContactoBulkAsync(UpsertEntidadeContactoBulkRequest request);
    }
}
