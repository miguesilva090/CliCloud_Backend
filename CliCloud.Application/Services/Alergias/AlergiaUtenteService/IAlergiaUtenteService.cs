using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.AlergiaUtenteService.DTOs;
using CliCloud.Application.Services.AlergiaUtenteService.Filters;

namespace CliCloud.Application.Services.AlergiaUtenteService
{
    public interface IAlergiaUtenteService : ITransientService
    {
        Task<Response<IEnumerable<AlergiaUtenteDTO>>> GetAlergiaUtenteAsync(string keyword = "");
        Task<PaginatedResponse<AlergiaUtenteDTO>> GetAlergiaUtentePaginatedAsync(AlergiaUtenteTableFilter filter);
        Task<Response<AlergiaUtenteDTO>> GetAlergiaUtenteAsync(Guid id);
        Task<Response<Guid>> CreateAlergiaUtenteAsync(CreateAlergiaUtenteRequest request);
        Task<Response<Guid>> UpdateAlergiaUtenteAsync(UpdateAlergiaUtenteRequest request, Guid id);
        Task<Response<Guid>> DeleteAlergiaUtenteAsync(Guid id);

    }
}
