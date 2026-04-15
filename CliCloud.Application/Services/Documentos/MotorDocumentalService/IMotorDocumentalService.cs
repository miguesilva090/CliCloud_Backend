
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;


namespace CliCloud.Application.Services.Documentos.MotorDocumentalService;

public interface IMotorDocumentalService : ITransientService
{
    Task<Response<IEnumerable<ModeloDocumentoDTO>>> ObterModelosAsync(string keyword = "");
    Task<Response<ModeloDocumentoDTO>> ObterModeloPorIdAsync(Guid id);
    Task<Response<Guid>> CriarModeloAsync(CriarModeloDocumentoRequest request);
    Task<Response<Guid>> AtualizarModeloAsync(AtualizarModeloDocumentoRequest request, Guid id);
    Task<Response<Guid>> PublicarNovaVersaoModeloAsync(Guid id);
    Task<Response<InstanciaDocumentoDTO>> GerarInstanciaAsync(GerarInstanciaDocumentoRequest request);
    Task<Response<byte[]>> ExportarModeloDocxAsync(Guid id);
    Task<Response<byte[]>> ExportarInstanciaDocxAsync(Guid id);
    Task<Response<IEnumerable<InstanciaDocumentoDTO>>> ObterInstanciasAsync(Guid? modeloId = null);
    Task<Response<Guid>> EliminarModeloAsync(Guid id);
    Task<Response<FicheiroDocumentoDTO>> RegistarFicheiroInstanciaAsync(RegistarFicheiroDocumentoRequest request);
    Task<Response<IEnumerable<FicheiroDocumentoDTO>>> ObterFicheirosInstanciaAsync(Guid instanciaId);
    Task<Response<FicheiroDocumentoDTO>> ObterFicheiroPorIdAsync(Guid ficheiroId);
    Task<Response<Guid>> EliminarFicheiroAsync(Guid ficheiroId);

}