using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Antecedentes
{
    [Route("client/antecedentes/[controller]")]
    [ApiController]
    public class AntecedentesCirurgicosController(IAntecedentesCirurgicosService antecedentesCirurgicosService) : ControllerBase
    {
        private readonly IAntecedentesCirurgicosService _antecedentesCirurgicosService = antecedentesCirurgicosService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAntecedentesCirurgicosAsync(string keyword = "")
        {
            Response<IEnumerable<AntecedentesCirurgicosDTO>> result = await _antecedentesCirurgicosService.GetAntecedentesCirurgicosAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAntecedentesCirurgicosPaginatedAsync(AntecedentesCirurgicosTableFilter filter)
        {
            PaginatedResponse<AntecedentesCirurgicosTableDTO> result = await _antecedentesCirurgicosService.GetAntecedentesCirurgicosPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAntecedentesCirurgicosAsync(Guid id)
        {
            Response<AntecedentesCirurgicosDTO> result = await _antecedentesCirurgicosService.GetAntecedentesCirurgicosAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAntecedentesCirurgicosAsync(CreateAntecedentesCirurgicosRequest request)
        {
            try
            {
                Response<Guid> result = await _antecedentesCirurgicosService.CreateAntecedentesCirurgicosAsync(request);
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
        public async Task<IActionResult> UpdateAntecedentesCirurgicosAsync(UpdateAntecedentesCirurgicosRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _antecedentesCirurgicosService.UpdateAntecedentesCirurgicosAsync(request, id);
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
        public async Task<IActionResult> DeleteAntecedentesCirurgicosAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _antecedentesCirurgicosService.DeleteAntecedentesCirurgicosAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
