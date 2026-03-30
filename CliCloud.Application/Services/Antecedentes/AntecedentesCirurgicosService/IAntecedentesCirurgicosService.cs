using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Filters;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService
{
    public interface IAntecedentesCirurgicosService : ITransientService
    {
        Task<Response<IEnumerable<AntecedentesCirurgicosDTO>>> GetAntecedentesCirurgicosAsync(string keyword = "");
        Task<PaginatedResponse<AntecedentesCirurgicosTableDTO>> GetAntecedentesCirurgicosPaginatedAsync(AntecedentesCirurgicosTableFilter filter);
        Task<Response<AntecedentesCirurgicosDTO>> GetAntecedentesCirurgicosAsync(Guid id);
        Task<Response<Guid>> CreateAntecedentesCirurgicosAsync(CreateAntecedentesCirurgicosRequest request);
        Task<Response<Guid>> UpdateAntecedentesCirurgicosAsync(UpdateAntecedentesCirurgicosRequest request, Guid id);
        Task<Response<Guid>> DeleteAntecedentesCirurgicosAsync(Guid id);

    }
}
