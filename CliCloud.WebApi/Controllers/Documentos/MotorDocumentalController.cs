using CliCloud.Application.Services.Documentos.MotorDocumentalService;
using CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using System.IO;

namespace CliCloud.WebApi.Controllers.Documentos;

[Route("client/documentos/motor")]
[ApiController]
public class MotorDocumentalController(IMotorDocumentalService service, IWebHostEnvironment env) : ControllerBase
{
    private readonly IMotorDocumentalService _service = service;
    private readonly IWebHostEnvironment _env = env;

    [Authorize(Roles = "client")]
    [HttpGet("modelos")]
    public async Task<IActionResult> ObterModelosAsync([FromQuery] string keyword = "")
        => Ok(await _service.ObterModelosAsync(keyword));

    [Authorize(Roles = "client")]
    [HttpGet("modelos/{id:guid}")]
    public async Task<IActionResult> ObterModeloPorIdAsync(Guid id)
        => Ok(await _service.ObterModeloPorIdAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost("modelos")]
    public async Task<IActionResult> CriarModeloAsync([FromBody] CriarModeloDocumentoRequest request)
        => Ok(await _service.CriarModeloAsync(request));

    [Authorize(Roles = "client")]
    [HttpPut("modelos/{id:guid}")]
    public async Task<IActionResult> AtualizarModeloAsync([FromBody] AtualizarModeloDocumentoRequest request, Guid id)
        => Ok(await _service.AtualizarModeloAsync(request, id));

    [Authorize(Roles = "client")]
    [HttpPost("modelos/{id:guid}/publicar")]
    public async Task<IActionResult> PublicarNovaVersaoModeloAsync(Guid id)
        => Ok(await _service.PublicarNovaVersaoModeloAsync(id));

    [Authorize(Roles = "client")]
    [HttpPost("instancias/gerar")]
    public async Task<IActionResult> GerarInstanciaAsync([FromBody] GerarInstanciaDocumentoRequest request)
        => Ok(await _service.GerarInstanciaAsync(request));

    [Authorize(Roles = "client")]
    [HttpDelete("modelos/{id:guid}")]
    public async Task<IActionResult> EliminarModeloAsync(Guid id)
        => Ok(await _service.EliminarModeloAsync(id));

    [Authorize(Roles = "client")]
    [HttpGet("instancias")]
    public async Task<IActionResult> ObterInstanciasAsync([FromQuery] Guid? modeloId = null)
        => Ok(await _service.ObterInstanciasAsync(modeloId));

    [Authorize(Roles = "client")]
    [HttpGet("modelos/{id:guid}/download-docx")]
    public async Task<IActionResult> DownloadModeloDocxAsync(Guid id)
    {
        var response = await _service.ExportarModeloDocxAsync(id);
        if (response.Status != CliCloud.Application.Common.Wrapper.ResponseStatus.Success || response.Data == null)
        {
            return BadRequest(response);
        }
        return File(response.Data, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"documento_{id}.docx");
    }

    [Authorize(Roles = "client")]
    [HttpGet("instancias/{id:guid}/download-docx")]
    public async Task<IActionResult> DownloadInstanciaDocxAsync(Guid id)
    {
        var response = await _service.ExportarInstanciaDocxAsync(id);
        if (response.Status != CliCloud.Application.Common.Wrapper.ResponseStatus.Success || response.Data == null)
        {
            return BadRequest(response);
        }
        return File(response.Data, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"instancia_{id}.docx");
    }
    
    [Authorize(Roles = "client")]
    [HttpGet("instancias/{instanciaId:guid}/ficheiros")]
    public async Task<IActionResult> ObterFicheirosInstanciaAsync(Guid instanciaId) => 
        Ok(await _service.ObterFicheirosInstanciaAsync(instanciaId));

    [Authorize(Roles = "client")]
    [HttpPost("instancias/{instanciaId:guid}/ficheiros/upload")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<IActionResult> UploadFicheiroIntanciaAsync(Guid instanciaId, [FromForm] IFormFile file)
    {
        if(file == null || file.Length == 0)
            return BadRequest("Nenhum ficheiro enviado.");

        string extension = Path.GetExtension(file.FileName);
        string nomeArmazenamento = $"{Guid.NewGuid()}{extension}";

        string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string relativeFolder = Path.Combine("uploads", "documentos-motor", instanciaId.ToString());
        string physicalFolder = Path.Combine(webRoot, relativeFolder);
        Directory.CreateDirectory(physicalFolder);

        string physicalPath = Path.Combine(physicalFolder, nomeArmazenamento);

        await using (FileStream stream = new(physicalPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string checksum;
        await using (FileStream readStream = new(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            byte[] hash = await SHA256.HashDataAsync(readStream);
            checksum = Convert.ToHexString(hash);
        }

        string caminhoRelativo = Path.Combine(relativeFolder, nomeArmazenamento).Replace("\\", "/");

        var registerResponse = await _service.RegistarFicheiroInstanciaAsync(new RegistarFicheiroDocumentoRequest{
            InstanciaDocumentoId = instanciaId,
            NomeOriginal = file.FileName,
            NomeArmazenamento = nomeArmazenamento,
            CaminhoRelativo = caminhoRelativo,
            TipoMime = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            TamanhoBytes = file.Length,
            ChecksumSha256 = checksum
        });

        if(registerResponse.Status != CliCloud.Application.Common.Wrapper.ResponseStatus.Success )
        {
            if(System.IO.File.Exists(physicalPath))
                System.IO.File.Delete(physicalPath);
        }

        return Ok(registerResponse);
    }

    [Authorize(Roles = "client")]
    [HttpGet("ficheiros/{ficheiroId:guid}/download")]
    public async Task<IActionResult> DownloadFicheiroAsync(Guid ficheiroId)
    {
        var response = await _service.ObterFicheiroPorIdAsync(ficheiroId);
        if(response.Status != CliCloud.Application.Common.Wrapper.ResponseStatus.Success || response.Data == null)
        {
            return BadRequest(response);
        }

        string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string physicalPath = Path.Combine(webRoot, response.Data.CaminhoRelativo.Replace("/", Path.DirectorySeparatorChar.ToString()));

        if(!System.IO.File.Exists(physicalPath))
            return NotFound("Ficheiro não encontrado.");
        
        byte[] bytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
        return File(bytes, response.Data.TipoMime, response.Data.NomeOriginal);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("ficheiros/{ficheiroId:guid}")]
    public async Task<IActionResult> EliminarFicheiroAsync(Guid ficheiroId)
    {
        var ficheiroResponse = await _service.ObterFicheiroPorIdAsync(ficheiroId);
        if(ficheiroResponse.Status != CliCloud.Application.Common.Wrapper.ResponseStatus.Success || ficheiroResponse.Data == null)
        {
            return Ok(ficheiroResponse);
        }

        var deleteResponse = await _service.EliminarFicheiroAsync(ficheiroId);
        if(deleteResponse.Status == CliCloud.Application.Common.Wrapper.ResponseStatus.Success)
        {
            string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string physicalPath = Path.Combine(webRoot, ficheiroResponse.Data.CaminhoRelativo.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if(System.IO.File.Exists(physicalPath))
                System.IO.File.Delete(physicalPath);
        }

        return Ok(deleteResponse);
    }
}
