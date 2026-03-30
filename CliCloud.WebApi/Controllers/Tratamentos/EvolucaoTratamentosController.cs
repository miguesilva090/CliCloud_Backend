using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class EvolucaoTratamentosController(IEvolucaoTratamentoService EvolucaoTratamentoService) : ControllerBase
    {
        private readonly IEvolucaoTratamentoService _EvolucaoTratamentoService = EvolucaoTratamentoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEvolucaoTratamentoAsync(string keyword = "")
        {
            Response<IEnumerable<EvolucaoTratamentoDTO>> result = await _EvolucaoTratamentoService.GetEvolucaoTratamentoAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetEvolucaoTratamentoLightAsync(string keyword = "")
        {
            Response<IEnumerable<EvolucaoTratamentoLightDTO>> result = await _EvolucaoTratamentoService.GetEvolucaoTratamentoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEvolucaoTratamentoPaginatedAsync([FromBody] EvolucaoTratamentoTableFilter filter)
        {
            PaginatedResponse<EvolucaoTratamentoTableDTO> result = await _EvolucaoTratamentoService.GetEvolucaoTratamentoPaginatedAsync(filter);
            return Ok(result);
        }

        // all list (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEvolucaoTratamentoAsync([FromBody] EvolucaoTratamentoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<EvolucaoTratamentoTableDTO>> result = await _EvolucaoTratamentoService.GetAllEvolucaoTratamentoAsync(filter);
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
        public async Task<IActionResult> GetEvolucaoTratamentoAsync(Guid id)
        {
            Response<EvolucaoTratamentoDTO> result = await _EvolucaoTratamentoService.GetEvolucaoTratamentoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEvolucaoTratamentoAsync(CreateEvolucaoTratamentoRequest request)
        {
            try
            {
                Response<Guid> result = await _EvolucaoTratamentoService.CreateEvolucaoTratamentoAsync(request);
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
        public async Task<IActionResult> UpdateEvolucaoTratamentoAsync(UpdateEvolucaoTratamentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _EvolucaoTratamentoService.UpdateEvolucaoTratamentoAsync(request, id);
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
        public async Task<IActionResult> DeleteEvolucaoTratamentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EvolucaoTratamentoService.DeleteEvolucaoTratamentoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleEvolucaoTratamentoAsync([FromBody] DeleteMultipleEvolucaoTratamentoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _EvolucaoTratamentoService.DeleteMultipleEvolucaoTratamentoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // report 
        [Authorize(Roles = "client")]
        [HttpGet("{id}/report")]
        public async Task<IActionResult> GetEvolucaoTratamentoReportAsync(Guid id)
        {
            Response<EvolucaoTratamentoReportDTO> result = 
                await _EvolucaoTratamentoService.GetEvolucaoTratamentoReportAsync(id);

            if(result.Status == ResponseStatus.Failure)
            {
                if(result.Messages.TryGetValue("$", out var msgs) && 
                    msgs.Any(m => m.Contains("Nenhum registo de Evolução de Tratamento")))

                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
