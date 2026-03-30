using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.DTOs;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Filters;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService
{
    public interface IAntecedentesPessoaisService : ITransientService
    {
        Task<Response<IEnumerable<AntecedentesPessoaisDTO>>> GetAntecedentesPessoaisAsync(string keyword = "");
        Task<Response<IEnumerable<AntecedentesPessoaisLightDTO>>> GetAntecedentesPessoaisLightAsync(string keyword = "");
        Task<PaginatedResponse<AntecedentesPessoaisTableDTO>> GetAntecedentesPessoaisPaginatedAsync(AntecedentesPessoaisTableFilter filter);
        Task<Response<IEnumerable<AntecedentesPessoaisTableDTO>>> GetAllAntecedentesPessoaisAsync(AntecedentesPessoaisAllFilter filter);
        Task<Response<AntecedentesPessoaisDTO>> GetAntecedentesPessoaisAsync(Guid id);
        Task<Response<AntecedentesPessoaisDTO>> GetAntecedentesPessoaisByNomeDoencaAsync(string nomeDoenca);
        Task<Response<Guid>> CreateAntecedentesPessoaisAsync(CreateAntecedentesPessoaisRequest request);
        Task<Response<Guid>> UpdateAntecedentesPessoaisAsync(UpdateAntecedentesPessoaisRequest request, Guid id);
        Task<Response<Guid>> DeleteAntecedentesPessoaisAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAntecedentesPessoaisAsync(IEnumerable<Guid> ids);
    }
}