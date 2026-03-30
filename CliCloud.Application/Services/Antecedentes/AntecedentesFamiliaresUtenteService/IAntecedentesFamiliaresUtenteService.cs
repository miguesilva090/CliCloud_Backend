using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Filters;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService
{
    public interface IAntecedentesFamiliaresUtenteService : ITransientService
    {
        Task<Response<IEnumerable<AntecedentesFamiliaresUtenteDTO>>> GetAntecedentesFamiliaresUtenteAsync(string keyword = "");
        Task<Response<IEnumerable<AntecedentesFamiliaresUtenteLightDTO>>> GetAntecedentesFamiliaresUtenteLightAsync(string keyword = "");
        Task<PaginatedResponse<AntecedentesFamiliaresUtenteTableDTO>> GetAntecedentesFamiliaresUtentePaginatedAsync(AntecedentesFamiliaresUtenteTableFilter filter);
        Task<Response<IEnumerable<AntecedentesFamiliaresUtenteTableDTO>>> GetAllAntecedentesFamiliaresUtenteAsync(AntecedentesFamiliaresUtenteAllFilter filter);
        Task<Response<AntecedentesFamiliaresUtenteDTO>> GetAntecedentesFamiliaresUtenteAsync(Guid id);
        Task<Response<AntecedentesFamiliaresUtenteDTO>> GetAntecedentesFamiliaresUtenteByNomeDoencaAsync(string nomeDoenca);
        Task<Response<Guid>> CreateAntecedentesFamiliaresUtenteAsync(CreateAntecedentesFamiliaresUtenteRequest request);
        Task<Response<Guid>> UpdateAntecedentesFamiliaresUtenteAsync(UpdateAntecedentesFamiliaresUtenteRequest request, Guid id);
        Task<Response<Guid>> DeleteAntecedentesFamiliaresUtenteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAntecedentesFamiliaresUtenteAsync(IEnumerable<Guid> ids);
    }
}
