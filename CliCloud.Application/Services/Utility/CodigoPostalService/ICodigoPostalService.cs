using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.Filters;

namespace CliCloud.Application.Services.Utility.CodigoPostalService
{
    public interface ICodigoPostalService : ITransientService
    {
        Task<Response<IEnumerable<CodigoPostalDTO>>> GetCodigoPostalAsync(string keyword = "");
        Task<Response<IEnumerable<CodigoPostalLightDTO>>> GetCodigoPostalLightAsync(string keyword = "");
        Task<PaginatedResponse<CodigoPostalTableDTO>> GetCodigoPostalPaginatedAsync(CodigoPostalTableFilter filter);
        Task<Response<IEnumerable<CodigoPostalTableDTO>>> GetAllCodigoPostalAsync(CodigoPostalAllFilter filter);
        Task<Response<CodigoPostalDTO>> GetCodigoPostalAsync(Guid id);
        Task<Response<Guid>> CreateCodigoPostalAsync(CreateCodigoPostalRequest request);
        Task<Response<Guid>> UpdateCodigoPostalAsync(UpdateCodigoPostalRequest request, Guid id);
        Task<Response<Guid>> DeleteCodigoPostalAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleCodigoPostalAsync(IEnumerable<Guid> ids);
    }
}
