using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Filters;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService
{
    public interface IProvenienciaUtenteService : ITransientService
    {
        Task<Response<IEnumerable<ProvenienciaUtenteDTO>>> GetProvenienciaUtenteAsync(string keyword = "");
        Task<Response<IEnumerable<ProvenienciaUtenteLightDTO>>> GetProvenienciaUtenteLightAsync(string keyword = "");
        Task<PaginatedResponse<ProvenienciaUtenteTableDTO>> GetProvenienciaUtentePaginatedAsync(ProvenienciaUtenteTableFilter filter);
        Task<Response<IEnumerable<ProvenienciaUtenteTableDTO>>> GetAllProvenienciaUtenteAsync(ProvenienciaUtenteAllFilter filter);
        Task<Response<ProvenienciaUtenteDTO>> GetProvenienciaUtenteAsync(Guid id);
        Task<Response<Guid>> CreateProvenienciaUtenteAsync(CreateProvenienciaUtenteRequest request);
        Task<Response<Guid>> UpdateProvenienciaUtenteAsync(UpdateProvenienciaUtenteRequest request, Guid id);
        Task<Response<Guid>> DeleteProvenienciaUtenteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleProvenienciaUtenteAsync(IEnumerable<Guid> ids);
    }
}
