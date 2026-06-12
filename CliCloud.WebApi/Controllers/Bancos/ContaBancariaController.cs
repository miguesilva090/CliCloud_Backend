using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Bancos.ContaBancariaService;
using CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs;
using CliCloud.Application.Services.Bancos.ContaBancariaService.Filters;

namespace CliCloud.WebApi.Controllers.Bancos
{
    [Route("client/bancos/[controller]")]
    [ApiController]
    public class ContaBancariaController(IContaBancariaService contaBancariaService) : ControllerBase
    {
        private readonly IContaBancariaService _contaBancariaService = contaBancariaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetContaBancariaAsync(string keyword = "")
        {
            Response<IEnumerable<ContaBancariaDTO>> result =
                await _contaBancariaService.GetContaBancariaAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetContaBancariaLightAsync(string keyword = "")
        {
            Response<IEnumerable<ContaBancariaLightDTO>> result =
                await _contaBancariaService.GetContaBancariaLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetContaBancariaPaginatedAsync(ContaBancariaTableFilter filter)
        {
            PaginatedResponse<ContaBancariaTableDTO> result =
                await _contaBancariaService.GetContaBancariaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllContaBancariaAsync([FromBody] ContaBancariaAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<ContaBancariaTableDTO>> result =
                    await _contaBancariaService.GetAllContaBancariaAsync(filter ?? new ContaBancariaAllFilter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContaBancariaByIdAsync(Guid id)
        {
            Response<ContaBancariaDTO> result = await _contaBancariaService.GetContaBancariaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateContaBancariaAsync(CreateContaBancariaRequest request)
        {
            try
            {
                Response<Guid> result = await _contaBancariaService.CreateContaBancariaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContaBancariaAsync(
            [FromRoute] Guid id,
            [FromBody] UpdateContaBancariaRequest request)
        {
            try
            {
                Response<Guid> result = await _contaBancariaService.UpdateContaBancariaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContaBancariaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _contaBancariaService.DeleteContaBancariaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleContaBancariaAsync(
            [FromBody] DeleteMultipleContaBancariaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result =
                    await _contaBancariaService.DeleteMultipleContaBancariaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}