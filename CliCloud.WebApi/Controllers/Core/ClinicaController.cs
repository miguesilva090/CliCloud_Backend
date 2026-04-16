using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ClinicaService;
using CliCloud.Application.Services.Core.ClinicaService.DTOs;
using CliCloud.Application.Services.Core.ClinicaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Core
{
    [Route("client/core/[controller]")]
    [ApiController]
    public class ClinicaController(IClinicaService ClinicaService, ICurrentClinicaService currentClinica) : ControllerBase
    {
        private readonly IClinicaService _ClinicaService = ClinicaService;
        private readonly ICurrentClinicaService _currentClinica = currentClinica;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetClinicaAsync(string keyword = "")
        {
            Response<IEnumerable<ClinicaDTO>> result = await _ClinicaService.GetClinicaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetClinicaLightAsync(string keyword = "")
        {
          Response<IEnumerable<ClinicaLightDTO>> result = await _ClinicaService.GetClinicaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetClinicaPaginatedAsync(ClinicaTableFilter filter)
        {
            PaginatedResponse<ClinicaTableDTO> result = await _ClinicaService.GetClinicaPaginatedAsync(filter);
            return Ok(result);
        }

        // All Clinicas (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllClinicaAsync([FromBody] ClinicaAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<ClinicaTableDTO>> result = await _ClinicaService.GetAllClinicaAsync(filter ?? new ClinicaAllFilter());
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClinicaAsync(Guid id)
        {
            Response<ClinicaDTO> result = await _ClinicaService.GetClinicaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateClinicaAsync(CreateClinicaRequest request)
        {
            try
            {
                Response<Guid> result = await _ClinicaService.CreateClinicaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinicaAsync(UpdateClinicaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ClinicaService.UpdateClinicaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinicaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ClinicaService.DeleteClinicaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleClinicaAsync([FromBody] DeleteMultipleClinicaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _ClinicaService.DeleteMultipleClinicaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // current clinic (tenant) from JWT claim "clinica_id"
        [Authorize(Roles = "client")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentClinicaAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) || !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
                return BadRequest("Clínica atual inválida.");

            Response<ClinicaDTO> result = await _ClinicaService.GetClinicaAsync(clinicaId);
            return Ok(result);
        }

        // update current clinic (tenant) from JWT claim "clinica_id"
        [Authorize(Roles = "client")]
        [HttpPut("current")]
        public async Task<IActionResult> UpdateCurrentClinicaAsync([FromBody] UpdateClinicaRequest request)
        {
            await _currentClinica.SetClinicaAsync();
            if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) || !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
                return BadRequest("Clínica atual inválida.");

            Response<Guid> result = await _ClinicaService.UpdateClinicaAsync(request, clinicaId);
            return Ok(result);
        }

        // set default clinic (legacy: EMPRESAS.pordefeito)
        [Authorize(Roles = "client")]
        [HttpPut("{id}/default")]
        public async Task<IActionResult> SetDefaultClinicaAsync(
            Guid id,
            [FromQuery] bool porDefeito = true
        )
        {
            Response<Guid> result = await _ClinicaService.SetDefaultClinicaAsync(id, porDefeito);
            return Ok(result);
        }

        // ---------------------------
        // Legacy: WSComum.asmx outputs
        // ---------------------------

        // Legacy: WSComum.asmx/ObterAvisosClinica
        [Authorize(Roles = "client")]
        [HttpGet("current/avisos")]
        public async Task<IActionResult> GetAvisosClinicaCurrentAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) ||
                !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
                return BadRequest("Clínica atual inválida.");

            Response<AvisosClinicaLegacyDTO> result = await _ClinicaService.GetAvisosClinicaAsync(clinicaId);
            return Ok(result);
        }

        // Legacy: WSComum.asmx/ObterFolgasClinica
        [Authorize(Roles = "client")]
        [HttpGet("current/folgas")]
        public async Task<IActionResult> GetFolgasClinicaCurrentAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) ||
                !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
                return BadRequest("Clínica atual inválida.");

            Response<int[]> result = await _ClinicaService.GetFolgasClinicaAsync(clinicaId);
            return Ok(result);
        }

        // Legacy: WSComum.asmx/obterPortaCartao
        [Authorize(Roles = "client")]
        [HttpGet("current/porta-cartao")]
        public async Task<IActionResult> GetPortaCartaoClinicaCurrentAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) ||
                !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
                return BadRequest("Clínica atual inválida.");

            Response<int?> result = await _ClinicaService.GetPortaCartaoClinicaAsync(clinicaId);
            return Ok(result);
        }

        // Legacy: WSComum.asmx/ClinicasAutocomplete
        [Authorize(Roles = "client")]
        [HttpGet("autocomplete")]
        public async Task<IActionResult> GetClinicasAutocompleteAsync([FromQuery] string? q = "")
        {
            Response<IEnumerable<AutoCompleteItemDTO>> result =
              await _ClinicaService.GetClinicasAutocompleteAsync(q);
            return Ok(result);
        }

        // Legacy: WSComum.asmx/ClinicasSelectedAutocomplete
        [Authorize(Roles = "client")]
        [HttpGet("autocomplete-selected")]
        public async Task<IActionResult> GetClinicasSelectedAutocompleteAsync([FromQuery] string? q = "")
        {
            await _currentClinica.SetClinicaAsync();
            if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) ||
                !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
                return BadRequest("Clínica atual inválida.");

            Response<IEnumerable<AutoCompleteItemDTO>> result =
              await _ClinicaService.GetClinicasSelectedAutocompleteAsync(q, clinicaId);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("current/configuracao-ano-ativa")]
        public async Task<IActionResult> GetConfiguracaoAnoAtivaCurrentAsync()
        {
            await _currentClinica.SetClinicaAsync();
            if(string.IsNullOrWhiteSpace(_currentClinica.ClinicaId) || !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId))
            return BadRequest("Clínica atual inválida");

            var result = await _ClinicaService.GetConfiguracaoAnoAtivaAsync(clinicaId);
            return Ok(result);
        }
    }
}
