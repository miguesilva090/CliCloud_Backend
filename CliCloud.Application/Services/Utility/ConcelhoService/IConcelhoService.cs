using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.Filters;

namespace CliCloud.Application.Services.Utility.ConcelhoService
{
    public interface IConcelhoService : ITransientService
    {
        Task<Response<IEnumerable<ConcelhoDTO>>> GetConcelhoAsync(string keyword = "");
        Task<Response<IEnumerable<ConcelhoLightDTO>>> GetConcelhoLightAsync(string keyword = "");
        Task<PaginatedResponse<ConcelhoTableDTO>> GetConcelhoPaginatedAsync(ConcelhoTableFilter filter);
        Task<Response<IEnumerable<ConcelhoTableDTO>>> GetAllConcelhoAsync(ConcelhoAllFilter filter);
        Task<Response<ConcelhoDTO>> GetConcelhoAsync(Guid id);
        Task<Response<Guid>> CreateConcelhoAsync(CreateConcelhoRequest request);
        Task<Response<Guid>> UpdateConcelhoAsync(UpdateConcelhoRequest request, Guid id);
        Task<Response<Guid>> DeleteConcelhoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleConcelhoAsync(IEnumerable<Guid> ids);
    }
}
