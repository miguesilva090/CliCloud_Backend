using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Filters;

namespace CliCloud.Application.Services.Utentes.UtenteService
{
    public interface IUtenteService : ITransientService
    {
        Task<Response<IEnumerable<UtenteDTO>>> GetUtenteAsync(string keyword = "");
        Task<Response<IEnumerable<UtenteLightDTO>>> GetUtenteLightAsync(string keyword = "");
        Task<PaginatedResponse<UtenteTableDTO>> GetUtentePaginatedAsync(UtenteTableFilter filter);
        Task<Response<IEnumerable<UtenteTableDTO>>> GetAllUtenteAsync(UtenteAllFilter filter);
        Task<Response<UtenteDTO>> GetUtenteAsync(Guid id);
        Task<Response<UtenteDTO>> GetUtenteByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<UtenteDTO>>> GetUtenteByNameAsync(string nome);
        Task<Response<UtenteDTO>> GetUtenteByNumeroUtenteAsync(string numeroUtente);
        Task<Response<Guid>> CreateUtenteAsync(CreateUtenteRequest request);
        Task<Response<Guid>> UpdateUtenteAsync(UpdateUtenteRequest request, Guid id);
        Task<Response<Guid>> DeleteUtenteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleUtenteAsync(IEnumerable<Guid> ids);
    }
}
