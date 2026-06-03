#nullable enable

using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

public interface IDocumentoEmissaoService : ITransientService
{
    Task<Response<DocumentoEmissaoOpcoesPagamentoDTO>> GetOpcoesPagamentoAsync();
    Task<Response<SinistradosInfoFaturacaoResponse>> SinistradosInfoFaturacaoAsync(
        SinistradosInfoFaturacaoRequest request);
    Task<Response<FaturaGlobalObterResponse>> FaturaGlobalObterAsync(FaturaGlobalObterRequest request);
    Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoAsync(EmitirDocumentoRequest request);
    Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoDesdeAdmissaoAsync(Guid admissaoId , EmitirDocumentoDesdeAdmissaoRequest request);
    Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoDesdeConsultaAsync(Guid consultaId, EmitirDocumentoDesdeConsultaRequest request);
    Task<Response<Guid>> AnularDocumentoAsync(Guid documentoId, AnularDocumentoRequest request);
    Task<Response<DocumentoEmissaoDTO>> CriarNotaCreditoAsync(CriarNotaCreditoRequest request);
}