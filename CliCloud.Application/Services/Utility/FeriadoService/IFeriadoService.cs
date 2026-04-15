using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.FeriadoService.DTOs;
using CliCloud.Application.Services.Utility.FeriadoService.Filters;

namespace CliCloud.Application.Services.Utility.FeriadoService;

public interface IFeriadoService : ITransientService
{
    Task<Response<IEnumerable<FeriadoDTO>>> GetTodosAsync(string keyword = "");
    Task<PaginatedResponse<FeriadoDTO>> GetPaginadoAsync(FeriadoTableFilter filter);
    Task<Response<FeriadoDTO>> GetPorIdAsync(Guid id);

    Task<Response<Guid>> CriarAsync(CreateFeriadoRequest request);
    Task<Response<Guid>> AtualizarAsync(UpdateFeriadoRequest request, Guid id);
    Task<Response<Guid>> ApagarAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> ApagarEmLoteAsync(IEnumerable<Guid> ids);

    Task<Response<int>> InserirAnoAsync(InsertFeriadosAnoRequest request);
    Task<Response<int>> ImportarAsync(ImportFeriadosRequest request);

    Task<Response<bool>> VerificarSeEFeriadoAsync(DateTime data);
}