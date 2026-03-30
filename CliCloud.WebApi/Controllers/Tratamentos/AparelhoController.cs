using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.AparelhoService;
using CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.AparelhoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class AparelhoController(IAparelhoService AparelhoService) : ControllerBase
    {
        private readonly IAparelhoService _AparelhoService = AparelhoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAparelhoAsync(string keyword = "")
        {
            Response<IEnumerable<AparelhoDTO>> result = await _AparelhoService.GetAparelhoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAparelhoLightAsync(string keyword = "")
        {
          Response<IEnumerable<AparelhoLightDTO>> result = await _AparelhoService.GetAparelhoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAparelhoPaginatedAsync(AparelhoTableFilter filter)
        {
            PaginatedResponse<AparelhoTableDTO> result = await _AparelhoService.GetAparelhoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Aparelhos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAparelhoAsync([FromBody] AparelhoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<AparelhoTableDTO>> result = await _AparelhoService.GetAllAparelhoAsync(filter);
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
        public async Task<IActionResult> GetAparelhoAsync(Guid id)
        {
            Response<AparelhoDTO> result = await _AparelhoService.GetAparelhoAsync(id);
            return Ok(result);
        }

        // single by CodigoSerie (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("codigo-serie/{codigoSerie}")]
        public async Task<IActionResult> GetAparelhoByCodigoSerieAsync(string codigoSerie)
        {
            try
            {
                Response<AparelhoDTO> result = await _AparelhoService.GetAparelhoByCodigoSerieAsync(codigoSerie);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAparelhoAsync(CreateAparelhoRequest request)
        {
            try
            {
                Response<Guid> result = await _AparelhoService.CreateAparelhoAsync(request);
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
        public async Task<IActionResult> UpdateAparelhoAsync(UpdateAparelhoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _AparelhoService.UpdateAparelhoAsync(request, id);
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
        public async Task<IActionResult> DeleteAparelhoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _AparelhoService.DeleteAparelhoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleAparelhoAsync([FromBody] DeleteMultipleAparelhoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _AparelhoService.DeleteMultipleAparelhoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
