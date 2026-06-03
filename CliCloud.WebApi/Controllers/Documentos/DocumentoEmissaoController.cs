#nullable enable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

namespace CliCloud.WebApi.Controllers.Documentos;

[Route("client/documentos/[controller]")]
[ApiController]
public class DocumentoEmissaoController(IDocumentoEmissaoService DocumentoEmissaoService) : ControllerBase
{
    [Authorize(Roles = "client")]
    [HttpGet("opcoes/pagamento")]
    public async Task<IActionResult> GetOpcoesPagamento()
    {
        Response<DocumentoEmissaoOpcoesPagamentoDTO> result =
            await DocumentoEmissaoService.GetOpcoesPagamentoAsync();
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("sinistrados/info-faturacao")]
    public async Task<IActionResult> SinistradosInfoFaturacao(
        [FromBody] SinistradosInfoFaturacaoRequest request)
    {
        Response<SinistradosInfoFaturacaoResponse> result =
            await DocumentoEmissaoService.SinistradosInfoFaturacaoAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("fatura-global/obter")]
    public async Task<IActionResult> FaturaGlobalObter([FromBody] FaturaGlobalObterRequest request)
    {
        Response<FaturaGlobalObterResponse> result =
            await DocumentoEmissaoService.FaturaGlobalObterAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("emitir")]
    public async Task<IActionResult> Emitir([FromBody] EmitirDocumentoRequest request)
    {
        Response<DocumentoEmissaoDTO> result = await DocumentoEmissaoService.EmitirDocumentoAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("emitir/admissao/{admissaoId:guid}")]
    public async Task<IActionResult> EmitirDesdeAdmissao(Guid admissaoId, [FromBody] EmitirDocumentoDesdeAdmissaoRequest request)
    {
        Response<DocumentoEmissaoDTO> result = 
            await DocumentoEmissaoService.EmitirDocumentoDesdeAdmissaoAsync(admissaoId, request);

        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("emitir/consulta/{consultaId:guid}")]
    public async Task<IActionResult> EmitirDesdeConsulta(Guid consultaId, [FromBody] EmitirDocumentoDesdeConsultaRequest request)
    {
        Response<DocumentoEmissaoDTO> result =
            await DocumentoEmissaoService.EmitirDocumentoDesdeConsultaAsync(consultaId, request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("anular/{documentoId:guid}")]
    public async Task<IActionResult> AnularDocumento(Guid documentoId, [FromBody] AnularDocumentoRequest request)
    {
        Response<Guid> result = await DocumentoEmissaoService.AnularDocumentoAsync(documentoId, request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("nota-credito")]
    public async Task<IActionResult> CriarNotaCredito([FromBody] CriarNotaCreditoRequest request)
    {
        Response<DocumentoEmissaoDTO> result = await DocumentoEmissaoService.CriarNotaCreditoAsync(request);
        return Ok(result);
    }
}