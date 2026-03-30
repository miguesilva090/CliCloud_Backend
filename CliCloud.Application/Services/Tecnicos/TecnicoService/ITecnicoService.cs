using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tecnicos.TecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.TecnicoService.Filters;

namespace CliCloud.Application.Services.Tecnicos.TecnicoService
{
    public interface ITecnicoService : ITransientService
    {
        Task<Response<IEnumerable<TecnicoDTO>>> GetTecnicoAsync(string keyword = "");
        Task<Response<IEnumerable<TecnicoLightDTO>>> GetTecnicoLightAsync(string keyword = "");
        Task<PaginatedResponse<TecnicoTableDTO>> GetTecnicoPaginatedAsync(TecnicoTableFilter filter);
        Task<Response<IEnumerable<TecnicoTableDTO>>> GetAllTecnicoAsync(TecnicoAllFilter filter);
        Task<Response<TecnicoDTO>> GetTecnicoAsync(Guid id);
        Task<Response<TecnicoDTO>> GetTecnicoByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<TecnicoDTO>>> GetTecnicoByNameAsync(string nome);
        Task<Response<Guid>> CreateTecnicoAsync(CreateTecnicoRequest request);
        Task<Response<Guid>> UpdateTecnicoAsync(UpdateTecnicoRequest request, Guid id);
        Task<Response<Guid>> DeleteTecnicoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTecnicoAsync(IEnumerable<Guid> ids);
    }
}
