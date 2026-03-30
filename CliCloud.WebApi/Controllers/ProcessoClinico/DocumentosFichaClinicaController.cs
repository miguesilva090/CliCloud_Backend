using CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService;
using CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/documentos-ficha-clinica")]
    [ApiController]
    [Authorize(Roles = "client")]
    public class DocumentosFichaClinicaController : ControllerBase
    {
        private readonly IDocumentosFichaClinicaService _documentosFichaClinicaService;
        private readonly IWebHostEnvironment _env;

        public DocumentosFichaClinicaController(
            IDocumentosFichaClinicaService documentosFichaClinicaService,
            IWebHostEnvironment env)
        {
            _documentosFichaClinicaService = documentosFichaClinicaService;
            _env = env;
        }

        // lista de documentos por utente (categoria opcional, default Clinico)
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetByUtenteAsync(Guid utenteId, [FromQuery] string? categoria = "Clinico")
        {
            Response<IEnumerable<DocumentosFichaClinicaDTO>> result =
                await _documentosFichaClinicaService.GetByUtenteAsync(utenteId, categoria);

            return Ok(result);
        }

        // upload + criação de registo
        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50 MB
        public async Task<IActionResult> UploadAsync(
            [FromForm] CreateDocumentosFichaClinicaRequest request,
            [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum ficheiro enviado.");
            }

            string extension = Path.GetExtension(file.FileName);
            string terminacao = extension.TrimStart('.').ToLowerInvariant();

            // mapear extensão para tipo lógico
            string tipo = extension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" => "Foto",
                ".mp4" or ".mov" or ".avi" or ".mkv" => "Video",
                _ => "Documento"
            };
            bool isVideo = tipo == "Video";

            string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string relativeFolder = Path.Combine("uploads", "documentos-ficha-clinica", request.UtenteId.ToString());
            string physicalFolder = Path.Combine(webRoot, relativeFolder);
            Directory.CreateDirectory(physicalFolder);

            string fileName = $"{Guid.NewGuid()}{extension}";
            string physicalPath = Path.Combine(physicalFolder, fileName);

            await using (FileStream stream = new(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string caminhoRelativo = Path.Combine(relativeFolder, fileName).Replace("\\", "/");

            Response<Guid> result = await _documentosFichaClinicaService.CreateAsync(
                request,
                fileName,
                caminhoRelativo,
                terminacao,
                tipo,
                isVideo);

            return Ok(result);
        }

        // apagar único
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            Response<Guid> response = await _documentosFichaClinicaService.DeleteAsync(id);
            return Ok(response);
        }

        // apagar múltiplos
        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleDocumentosFichaClinicaRequest request)
        {
            Response<IEnumerable<Guid>> response =
                await _documentosFichaClinicaService.DeleteMultipleAsync(request.Ids.ToList());

            return Ok(response);
        }
    }
}

