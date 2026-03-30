using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Exames.AcordosService;
using CliCloud.Application.Services.Exames.AcordosService.DTOs;
using CliCloud.Application.Services.Exames.AcordosService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Exames
{
    [Route("client/exames/[controller]")]
    [ApiController]
    public class AcordosController(IAcordosService AcordosService) : ControllerBase
    {
        private readonly IAcordosService _AcordosService = AcordosService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAcordosAsync(string keyword = "")
        {
            Response<IEnumerable<AcordosDTO>> result = await _AcordosService.GetAcordosAsync(keyword);
            return Ok(result);
        }

        //  lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAcordosLightAsync(string keyword = "") 
        {
            Response<IEnumerable<AcordosLightDTO>> result = await _AcordosService.GetAcordosLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAcordosPaginatedAsync(AcordosTableFilter filter)
        {
            PaginatedResponse<AcordosTableDTO> result = await _AcordosService.GetAcordosPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAcordosAsync([FromBody] AcordosAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<AcordosTableDTO>> result = await _AcordosService.GetAllAcordosAsync(filter ?? new AcordosAllFilter());
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
        public async Task<IActionResult> GetAcordosAsync(Guid id)
        {
            Response<AcordosDTO> result = await _AcordosService.GetAcordosAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAcordosAsync(CreateAcordosRequest request)
        {
            try
            {
                Response<Guid> result = await _AcordosService.CreateAcordosAsync(request);
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
        public async Task<IActionResult> UpdateAcordosAsync(UpdateAcordosRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _AcordosService.UpdateAcordosAsync(request, id);
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
        public async Task<IActionResult> DeleteAcordosAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _AcordosService.DeleteAcordosAsync(id);
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
        public async Task<IActionResult> DeleteMultipleAcordosAsync([FromBody] DeleteMultipleAcordosRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _AcordosService.DeleteMultipleAcordosAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
