using CliCloud.Application.Services.Documentos.ConsentimentoService;
using CliCloud.Application.Services.Documentos.ConsentimentoService.DTOs;
using CliCloud.Application.Services.Documentos.MotorDocumentalService;
using CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CliCloud.WebApi.Controllers.Documentos;

[Route("client/documentos/consentimentos")]
[ApiController]
[Authorize(Roles = "client")]
public class ConsentimentoController(
    IConsentimentoService service,
    IMotorDocumentalService motorDocumentalService,
    IWebHostEnvironment env
) : ControllerBase
{
    private readonly IConsentimentoService _service = service;
    private readonly IMotorDocumentalService _motorDocumentalService = motorDocumentalService;
    private readonly IWebHostEnvironment _env = env;

    [HttpPost("pedidos")]
    public async Task<IActionResult> CriarPedidoAsync([FromBody] CriarPedidoConsentimentoRequest request)
     => Ok(await _service.CriarPedidoAsync(request));

    [HttpGet("pedidos/{id:guid}")]
    public async Task<IActionResult> ObterPedidoAsync(Guid id)
     => Ok(await _service.ObterPedidoAsync(id));

    [HttpGet("pedidos")]
    public async Task<IActionResult> ObterPedidosAsync([FromQuery] Guid? utenteId = null, [FromQuery] int? estado = null)
     => Ok(await _service.ObterPedidosAsync(utenteId, estado));

    [HttpPost("pedidos/{id:guid}/cancelar")]
    public async Task<IActionResult> CancelarPedidoAsync(Guid id, [FromQuery] string? observacoes = null)
     => Ok(await _service.CancelarPedidoAsync(id, observacoes));

    [HttpPost("pedidos/{id:guid}/assinar")]
    public async Task<IActionResult> MarcarAssinadoAsync(Guid id,  [FromBody] MarcarPedidoConsentimentoAssinadoRequest request)
    {
        var signResponse = await _service.MarcarAssinadoAsync(id, request);
        if (signResponse.Status != CliCloud.Application.Common.Wrapper.ResponseStatus.Success)
        {
            return Ok(signResponse);
        }

        // Best-effort evidência de assinatura para aproximar trilho documental do legado.
        try
        {
            var pedidoResponse = await _service.ObterPedidoAsync(id);
            if (
                pedidoResponse.Status == CliCloud.Application.Common.Wrapper.ResponseStatus.Success
                && pedidoResponse.Data != null
            )
            {
                var pedido = pedidoResponse.Data;
                string assinaturaHash = string.IsNullOrWhiteSpace(request.AssinaturaBase64)
                    ? "SEM_ASSINATURA_BASE64"
                    : Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.AssinaturaBase64)));

                string evidenciaConteudo =
                    "EVIDENCIA_ASSINATURA_CONSENTIMENTO\n"
                    + $"PedidoId={pedido.Id}\n"
                    + $"InstanciaDocumentoId={pedido.InstanciaDocumentoId}\n"
                    + $"TipoConsentimento={pedido.TipoConsentimento}\n"
                    + $"Estado={pedido.Estado}\n"
                    + $"AssinadoPor={pedido.AssinadoPor}\n"
                    + $"AssinadoEm={pedido.AssinadoEm:O}\n"
                    + $"Canal={pedido.Canal}\n"
                    + $"UtenteId={pedido.UtenteId}\n"
                    + $"AssinaturaBase64SHA256={assinaturaHash}\n"
                    + $"UserAgent={Request.Headers.UserAgent}\n";

                byte[] evidenciaBytes = Encoding.UTF8.GetBytes(evidenciaConteudo);
                string ficheiroNome = $"consentimento-evidencia-{pedido.Id}.txt";

                string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string relativeFolder = Path.Combine("uploads", "documentos-motor", pedido.InstanciaDocumentoId.ToString());
                string physicalFolder = Path.Combine(webRoot, relativeFolder);
                Directory.CreateDirectory(physicalFolder);

                string physicalPath = Path.Combine(physicalFolder, ficheiroNome);
                await System.IO.File.WriteAllBytesAsync(physicalPath, evidenciaBytes);

                string ficheiroSha256 = Convert.ToHexString(SHA256.HashData(evidenciaBytes));
                string caminhoRelativo = Path.Combine(relativeFolder, ficheiroNome).Replace("\\", "/");

                _ = await _motorDocumentalService.RegistarFicheiroInstanciaAsync(new RegistarFicheiroDocumentoRequest
                {
                    InstanciaDocumentoId = pedido.InstanciaDocumentoId,
                    NomeOriginal = ficheiroNome,
                    NomeArmazenamento = ficheiroNome,
                    CaminhoRelativo = caminhoRelativo,
                    TipoMime = "text/plain",
                    TamanhoBytes = evidenciaBytes.LongLength,
                    ChecksumSha256 = ficheiroSha256
                });
            }
        }
        catch
        {
            // Não bloquear o fluxo de assinatura se a geração da evidência falhar.
        }

        return Ok(signResponse);
    }


}