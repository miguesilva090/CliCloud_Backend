using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Medicos.MargemMedicoService;
using CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.MargemMedicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Medicos
{
    [Route("client/medicos/[controller]")]
    [ApiController]
    public class MargemMedicoController(IMargemMedicoService MargemMedicoService) : ControllerBase
    {
        private readonly IMargemMedicoService _MargemMedicoService = MargemMedicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMargemMedicoAsync(string keyword = "")
        {
            Response<IEnumerable<MargemMedicoDTO>> result = await _MargemMedicoService.GetMargemMedicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMargemMedicoLightAsync(string keyword = "")
        {
            Response<IEnumerable<MargemMedicoLightDTO>> result = await _MargemMedicoService.GetMargemMedicoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMargemMedicoTablePaginatedAsync(MargemMedicoTableFilter filter)
        {
            PaginatedResponse<MargemMedicoTableDTO> result = await _MargemMedicoService.GetMargemMedicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All MargemMedicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMargemMedicoTableAsync([FromBody] MargemMedicoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<MargemMedicoTableDTO>> result = await _MargemMedicoService.GetAllMargemMedicoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMargemMedicoAsync(Guid id)
        {
            Response<MargemMedicoDTO> result = await _MargemMedicoService.GetMargemMedicoAsync(id);
            return Ok(result);
        }

        // Multiple by MedicoId
        [Authorize(Roles = "client")]
        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> GetMargemMedicoByMedicoIdAsync(Guid medicoId)
        {
            Response<IEnumerable<MargemMedicoDTO>> result = await _MargemMedicoService.GetMargemMedicoByMedicoIdAsync(medicoId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMargemMedicoAsync(CreateMargemMedicoRequest request)
        {
            try
            {
                Response<Guid> result = await _MargemMedicoService.CreateMargemMedicoAsync(request);
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
        public async Task<IActionResult> UpdateMargemMedicoAsync(UpdateMargemMedicoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _MargemMedicoService.UpdateMargemMedicoAsync(request, id);
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
        public async Task<IActionResult> DeleteMargemMedicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MargemMedicoService.DeleteMargemMedicoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleMargemMedicoAsync([FromBody] DeleteMultipleMargemMedicoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _MargemMedicoService.DeleteMultipleMargemMedicoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
