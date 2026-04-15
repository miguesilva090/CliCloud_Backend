using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService;

public interface ISeparadorPersonalizadoService : ITransientService
{
    Task<Response<IEnumerable<SeparadorPersonalizadoDTO>>> GetSeparadorPersonalizadoAsync(
        Guid clinicaId,
        string keyword = ""
    );
    Task<PaginatedResponse<SeparadorPersonalizadoDTO>> GetSeparadorPersonalizadoPaginatedAsync(
        Guid clinicaId,
        SeparadorPersonalizadoTableFilter filter
    );
    Task<Response<SeparadorPersonalizadoDTO>> GetSeparadorPersonalizadoAsync(Guid clinicaId, Guid id);
    Task<Response<Guid>> CreateSeparadorPersonalizadoAsync(
        Guid clinicaId,
        CreateSeparadorPersonalizadoRequest request
    );
    Task<Response<Guid>> UpdateSeparadorPersonalizadoAsync(
        Guid clinicaId,
        UpdateSeparadorPersonalizadoRequest request,
        Guid id
    );
    Task<Response<Guid>> DeleteSeparadorPersonalizadoAsync(Guid clinicaId, Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleSeparadorPersonalizadoAsync(
        Guid clinicaId,
        IEnumerable<Guid> ids
    );
}
