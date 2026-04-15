using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.Core.EmailService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.Email;

[Route("client/core/[controller]")]
[ApiController]
[Authorize(Roles = "client")]
public class ConfiguracaoEmailController(
    IConfiguracaoEmailService servico, 
    ICurrentClinicaService currentClinica) : ControllerBase
    {
        private readonly IConfiguracaoEmailService _servico = servico;
        private readonly ICurrentClinicaService _currentClinica = currentClinica;

        private async Task<Guid?> ObterClinicaIdAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if(string.IsNullOrWhiteSpace(_currentClinica.ClinicaId))
                return null;
            
            return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
        }

        [HttpGet("configuracao")]
        public async Task<IActionResult> ObterConfiguracaoAtualAsync()
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null)
                return BadRequest("Clínica atual inválida.");
            return Ok(await _servico.ObterConfiguracaoAtualAsync(clinicaId.Value));
        }

        [HttpPut("configuracao")]
        public async Task<IActionResult> GuardarConfiguracaoAsync([FromBody] AtualizarConfiguracaoEmailRequest request)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null)
                return BadRequest("Clínica atual inválida.");
            return Ok(await _servico.GuardarConfiguracaoAsync(clinicaId.Value, request));
        }

        [HttpGet("automaticos")]
        public async Task<IActionResult> ObterAutomaticosAsync()
        {
            var clinicaId = await ObterClinicaIdAsync();
            if ( clinicaId is null)
                return BadRequest("Clínica atual inválida.");

            return Ok(await _servico.ObterConfiguracoesAutomaticasAsync(clinicaId.Value));
        }

        [HttpGet("automaticos/{codigo}")]
        public async Task<IActionResult> ObterAutomaticoAsync(string codigo)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null)
                return BadRequest("Clínica atual inválida.");

            return Ok(await _servico.ObterConfiguracaoAutomaticaAsync(clinicaId.Value, codigo));
        }

        [HttpPut("automaticos/{codigo}")]
        public async Task<IActionResult> GuardarAutomaticoAsync(string codigo, [FromBody] AtualizarConfiguracaoEmailAutomaticaRequest request)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) 
                return BadRequest("Clínica atual inválida");
            request.Codigo = codigo;
            return Ok(await _servico.GuardarConfiguracaoAutomaticaAsync(clinicaId.Value, request));
        }

        [HttpPost("historico/paginado")]
        public async Task<IActionResult> ObterHistoricoPaginadoAsync([FromBody] HistoricoEmailTabelaFiltro filtro)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if (clinicaId is null)
                return BadRequest("Clínica atual inválida.");

            return Ok(await _servico.ObterHistoricoPaginadoAsync(clinicaId.Value, filtro));
        }

        [HttpGet("templates-fluxo")]
        public async Task<IActionResult> ObterTemplatesFluxoAsync()
        {
            var clinicaId = await ObterClinicaIdAsync();
            if (clinicaId is null)
                return BadRequest("Clínica atual inválida.");

            return Ok(await _servico.ObterTemplatesFluxoEmailAsync(clinicaId.Value));
        }

        [HttpPut("templates-fluxo")]
        public async Task<IActionResult> GuardarTemplatesFluxoAsync([FromBody] AtualizarTemplatesFluxoEmailRequest request)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if (clinicaId is null)
                return BadRequest("Clínica atual inválida.");

            return Ok(await _servico.GuardarTemplatesFluxoEmailAsync(clinicaId.Value, request));
        }

    }
