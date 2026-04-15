using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService;

public interface ISeparadorPersonalizadoDocumentoService : ITransientService
{
    Task<Response<SeparadorPersonalizadoModeloDTO>> GetModeloAsync(Guid separadorId);
    Task<Response<Guid>> UpsertModeloAsync(UpsertSeparadorPersonalizadoModeloRequest request);
    Task<Response<GerarImpressaoSeparadorPersonalizadoResponse>> GerarImpressaoAsync(
        GerarImpressaoSeparadorPersonalizadoRequest request
    );
}
