using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService;

public interface IReferenciasMbService : ITransientService
{
    Task<Response<ConfigReferenciaMbDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigReferenciaMbRequest request);
    Task<Response<string>> ConstruirCallbackIfThenAsync(Guid clinicaId);

    Task<Response<IEnumerable<ReferenciaMbTableDTO>>> ListarHistoricoAsync(Guid clinicaId);
    Task<Response<Guid>> MarcarLiquidadaAsync(Guid clinicaId, Guid referenciaId);
    Task<Response<Guid>> AnularAsync(Guid clinicaId, Guid referenciaId, AnularReferenciaMbRequest request);

    Task<Response<string>> ReceberCallbackIfThenAsync(IfThenCallbackRequest request);

    Task<Response<ReferenciaMbGeradaDTO>> GerarParaDocumentoAsync(GerarReferenciaDocumentoRequest request);
}