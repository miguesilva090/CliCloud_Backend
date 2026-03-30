using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Alergias.AlergiaService.DTOs;
using CliCloud.Application.Services.Alergias.AlergiaService.Filters;

namespace CliCloud.Application.Services.Alergias.AlergiaService
{
    public interface IAlergiaService : ITransientService
    {
        Task<Response<IEnumerable<AlergiaDTO>>> GetAlergiaAsync(string keyword = "");
        Task<Response<IEnumerable<AlergiaLightDTO>>> GetAlergiaLightAsync(string keyword = "");
        Task<PaginatedResponse<AlergiaTableDTO>> GetAlergiaPaginatedAsync(AlergiaTableFilter filter);
        Task<Response<AlergiaDTO>> GetAlergiaAsync(Guid id);
        Task<Response<Guid>> CreateAlergiaAsync(CreateAlergiaRequest request);
        Task<Response<Guid>> UpdateAlergiaAsync(UpdateAlergiaRequest request, Guid id);
        Task<Response<Guid>> DeleteAlergiaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAlergiaAsync(IEnumerable<Guid> ids);
    }
}
