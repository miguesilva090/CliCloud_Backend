using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Exames.AnalisesService.DTOs;
using CliCloud.Application.Services.Exames.AnalisesService.Filters;

namespace CliCloud.Application.Services.Exames.AnalisesService
{
    public interface IAnalisesService : ITransientService
    {
        Task<Response<IEnumerable<AnaliseDTO>>> GetAnaliseAsync(string keyword = "");
        Task<Response<IEnumerable<AnaliseLightDTO>>> GetAnaliseLightAsync(string keyword = "");
        Task<PaginatedResponse<AnaliseTableDTO>> GetAnalisePaginatedAsync(AnaliseTableFilter filter);
        Task<Response<IEnumerable<AnaliseTableDTO>>> GetAllAnaliseAsync(AnaliseAllFilter? filter);
        Task<Response<AnaliseDTO>> GetAnaliseAsync(Guid id);
        Task<Response<Guid>> CreateAnaliseAsync(CreateAnaliseRequest request);
        Task<Response<Guid>> UpdateAnaliseAsync(UpdateAnaliseRequest request, Guid id);
        Task<Response<Guid>> DeleteAnaliseAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAnaliseAsync(IEnumerable<Guid> ids);
    }
}
