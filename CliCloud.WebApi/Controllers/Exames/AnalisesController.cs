using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Exames.AnalisesService;
using CliCloud.Application.Services.Exames.AnalisesService.DTOs;
using CliCloud.Application.Services.Exames.AnalisesService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Exames
{
    [Route("client/exames/[controller]")]
    [ApiController]
    public class AnalisesController(IAnalisesService analisesService) : ControllerBase
    {
        private readonly IAnalisesService _analisesService = analisesService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnaliseAsync(string keyword = "")
        {
            Response<IEnumerable<AnaliseDTO>> result = await _analisesService.GetAnaliseAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAnaliseLightAsync(string keyword = "")
        {
            Response<IEnumerable<AnaliseLightDTO>> result = await _analisesService.GetAnaliseLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnalisePaginatedAsync(AnaliseTableFilter filter)
        {
            PaginatedResponse<AnaliseTableDTO> result = await _analisesService.GetAnalisePaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAnaliseAsync([FromBody] AnaliseAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<AnaliseTableDTO>> result = await _analisesService.GetAllAnaliseAsync(filter ?? new AnaliseAllFilter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnaliseAsync(Guid id)
        {
            Response<AnaliseDTO> result = await _analisesService.GetAnaliseAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnaliseAsync(CreateAnaliseRequest request)
        {
            try
            {
                Response<Guid> result = await _analisesService.CreateAnaliseAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnaliseAsync(UpdateAnaliseRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _analisesService.UpdateAnaliseAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnaliseAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _analisesService.DeleteAnaliseAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleAnaliseAsync([FromBody] DeleteMultipleAnaliseRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _analisesService.DeleteMultipleAnaliseAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
