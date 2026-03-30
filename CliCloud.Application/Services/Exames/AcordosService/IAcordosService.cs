using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Exames.AcordosService.DTOs;
using CliCloud.Application.Services.Exames.AcordosService.Filters;

namespace CliCloud.Application.Services.Exames.AcordosService
{
    public interface IAcordosService : ITransientService
    {
        Task<Response<IEnumerable<AcordosDTO>>> GetAcordosAsync(string keyword = "");
        Task<Response<IEnumerable<AcordosLightDTO>>> GetAcordosLightAsync(string keyword = "");
        Task<PaginatedResponse<AcordosTableDTO>> GetAcordosPaginatedAsync(AcordosTableFilter filter);
        Task<Response<IEnumerable<AcordosTableDTO>>> GetAllAcordosAsync(AcordosAllFilter? filter);
        Task<Response<AcordosDTO>> GetAcordosAsync(Guid id);
        Task<Response<Guid>> CreateAcordosAsync(CreateAcordosRequest request);
        Task<Response<Guid>> UpdateAcordosAsync(UpdateAcordosRequest request, Guid id);
        Task<Response<Guid>> DeleteAcordosAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAcordosAsync(IEnumerable<Guid> ids);
    }
}
