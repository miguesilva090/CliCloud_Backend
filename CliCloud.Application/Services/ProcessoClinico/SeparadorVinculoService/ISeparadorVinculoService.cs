using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService;

public interface ISeparadorVinculoService : ITransientService
{
    Task<Response<IEnumerable<SeparadorVinculoDTO>>> GetBySeparadorAsync(Guid separadorId);
    Task<Response<Guid>> CreateAsync(CreateSeparadorVinculoRequest request);
    Task<Response<Guid>> DeleteAsync(Guid id);
}
