using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.SmsService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace CliCloud.WebApi.Controllers.Core.Sms
{
    [Route("client/core/[controller]")]
    [ApiController]
    [Authorize(Roles = "client")]
    public class SmsController( IServicoSms servicoSms, ICurrentClinicaService currentClinica) : ControllerBase
    {
        private readonly IServicoSms _servicoSms = servicoSms;
        private readonly ICurrentClinicaService _currentClinica = currentClinica;

        private async Task<Guid?> ObterClinicaIdAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if( string.IsNullOrWhiteSpace(_currentClinica.ClinicaId))
                return null;

            return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
        }


        [HttpGet("configuracao")]
        public async Task<IActionResult> ObterConfiguracaoAtualAsync()
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.ObterConfiguracaoAtualAsync(clinicaId.Value);
            return Ok(result);
        }

        [HttpPut("configuracao")]
        public async Task<IActionResult> GuardarConfiguracaoAsync([FromBody] AtualizarConfiguracaoSmsRequest request)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.GuardarConfiguracaoAsync(clinicaId.Value, request);
            return Ok(result);
        }

        [HttpGet("automaticos")]
        public async Task<IActionResult> ObterConfiguracoesAutomaticasAsync()
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.ObterConfiguracoesAutomaticasAsync(clinicaId.Value);
            return Ok(result);
        }
        [HttpGet("automaticos/{codigo}")]
        public async Task<IActionResult> ObterConfiguracaoAutomaticaAsync( string codigo)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.ObterConfiguracaoAutomaticaAsync(clinicaId.Value, codigo);
            return Ok(result);
        }

        [HttpPut("automaticos/{codigo}")]
        public async Task<IActionResult> GuardarConfiguracaoAutomaticaAsync(string codigo, [FromBody] AtualizarConfiguracaoAutomaticaRequest request)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            request.Codigo = codigo ;
            var result = await _servicoSms.GuardarConfiguracaoAutomaticaAsync(clinicaId.Value, request);
            return Ok(result);
        }

        [HttpGet("automaticos/{codigo}/medicos")]
        public async Task<IActionResult> ObterMedicosSelecionadosAsync(string codigo)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.ObterMedicosSelecionadosAsync(clinicaId.Value, codigo);
            return Ok(result);
        }

        [HttpPut("automaticos/{codigo}/medicos")]
        public async Task<IActionResult> GuardarMedicosSelecionadosAsync(string codigo, [FromBody] GuardarMedicosSmsRequest request)
        {

            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            request.CodigoConfiguracao = codigo;
            var result = await _servicoSms.GuardarMedicosSelecionadosAsync(clinicaId.Value, request);
            return Ok(result);
        }

        [HttpPut("automaticos/{codigo}/todos-medicos")]
        public async Task<IActionResult> GuardarTodosMedicosAsync(
            string codigo , 
            [FromBody] GuardarTodosMedicosSmsRequest request
        )
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            request.CodigoConfiguracao = codigo;
            var result = await _servicoSms.GuardarTodosMedicosAsync(clinicaId.Value, request);
            return Ok(result);
        }

        [HttpPost("historico/paginado")]
        public async Task<IActionResult> ObterHistoricoPaginadoAsync(
            [FromBody] HistoricoSmsTabelaFiltro filtro 
        )
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.ObterHistoricoPaginadoAsync(clinicaId.Value, filtro);
            return Ok(result);
        }

        [HttpPost("enviar-teste")]
        public async Task<IActionResult> EnviarSmsTesteAsync([FromBody] EnviarSmsTesteRequest request)
        {
            var clinicaId = await ObterClinicaIdAsync();
            if(clinicaId is null) return BadRequest("Clínica atual inválida.");

            var result = await _servicoSms.EnviarSmsTesteAsync(clinicaId.Value, request);
            return Ok(result);
        }
    }
        
}