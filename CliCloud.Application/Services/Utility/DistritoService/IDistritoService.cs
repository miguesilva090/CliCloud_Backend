using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.Filters;

namespace CliCloud.Application.Services.Utility.DistritoService
{
    public interface IDistritoService : ITransientService
    {
        Task<Response<IEnumerable<DistritoDTO>>> GetDistritoAsync(string keyword = "");
        Task<Response<IEnumerable<DistritoLightDTO>>> GetDistritoLightAsync(string keyword = "", Guid? paisId = null);
        Task<PaginatedResponse<DistritoTableDTO>> GetDistritoPaginatedAsync(DistritoTableFilter filter);
        Task<Response<IEnumerable<DistritoTableDTO>>> GetAllDistritoAsync(DistritoAllFilter filter);
        Task<Response<DistritoDTO>> GetDistritoAsync(Guid id);
        Task<Response<Guid>> CreateDistritoAsync(CreateDistritoRequest request);
        Task<Response<Guid>> UpdateDistritoAsync(UpdateDistritoRequest request, Guid id);
        Task<Response<Guid>> DeleteDistritoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleDistritoAsync(IEnumerable<Guid> ids);
    }
}
