using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class HabitosEViciosController(IHabitosEViciosService HabitosEViciosService) : ControllerBase
    {
        private readonly IHabitosEViciosService _HabitosEViciosService = HabitosEViciosService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetHabitosEViciosAsync(string keyword = "")
        {
            Response<IEnumerable<HabitosEViciosDTO>> result = await _HabitosEViciosService.GetHabitosEViciosAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetHabitosEViciosLightAsync(string keyword = "")
        {
            Response<IEnumerable<HabitosEViciosLightDTO>> result = await _HabitosEViciosService.GetHabitosEViciosLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetHabitosEViciosPaginatedAsync(HabitosEViciosTableFilter filter)
        {
            PaginatedResponse<HabitosEViciosTableDTO> result = await _HabitosEViciosService.GetHabitosEViciosPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllHabitosEViciosAsync([FromBody] HabitosEViciosAllFilter filter)
        {
            Response<IEnumerable<HabitosEViciosTableDTO>> result = await _HabitosEViciosService.GetAllHabitosEViciosAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabitosEViciosAsync(Guid id)
        {
            Response<HabitosEViciosDTO> result = await _HabitosEViciosService.GetHabitosEViciosAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateHabitosEViciosAsync(CreateHabitosEViciosRequest request)
        {
            try
            {
                Response<Guid> result = await _HabitosEViciosService.CreateHabitosEViciosAsync(request);
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
        public async Task<IActionResult> UpdateHabitosEViciosAsync(UpdateHabitosEViciosRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _HabitosEViciosService.UpdateHabitosEViciosAsync(request, id);
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
        public async Task<IActionResult> DeleteHabitosEViciosAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _HabitosEViciosService.DeleteHabitosEViciosAsync(id);
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
        public async Task<IActionResult> DeleteMultipleHabitosEViciosAsync([FromBody] DeleteMultipleHabitosEViciosRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _HabitosEViciosService.DeleteMultipleHabitosEViciosAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
