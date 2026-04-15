using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService;

public interface ISeparadorPersonalizadoVinculoService : ITransientService
{
    Task<Response<IEnumerable<SeparadorPersonalizadoVinculoDTO>>> GetBySeparadorAsync(
        Guid clinicaId,
        Guid separadorPersonalizadoId
    );
    Task<Response<Guid>> CreateAsync(Guid clinicaId, CreateSeparadorPersonalizadoVinculoRequest request);
    Task<Response<Guid>> DeleteAsync(Guid clinicaId, Guid id);
}
