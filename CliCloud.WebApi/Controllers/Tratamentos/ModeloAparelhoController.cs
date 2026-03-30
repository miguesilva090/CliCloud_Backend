using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class ModeloAparelhoController(IModeloAparelhoService ModeloAparelhoService) : ControllerBase
    {
        private readonly IModeloAparelhoService _ModeloAparelhoService = ModeloAparelhoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetModeloAparelhoAsync(string keyword = "")
        {
            Response<IEnumerable<ModeloAparelhoDTO>> result = await _ModeloAparelhoService.GetModeloAparelhoAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetModeloAparelhoLightAsync(string keyword = "")
        {
            Response<IEnumerable<ModeloAparelhoLightDTO>> result = await _ModeloAparelhoService.GetModeloAparelhoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetModeloAparelhoPaginatedAsync(ModeloAparelhoTableFilter filter)
        {
            PaginatedResponse<ModeloAparelhoTableDTO> result = await _ModeloAparelhoService.GetModeloAparelhoPaginatedAsync(filter);
            return Ok(result);
        }

        // all list (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllModeloAparelhoAsync([FromBody] ModeloAparelhoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<ModeloAparelhoTableDTO>> result = await _ModeloAparelhoService.GetAllModeloAparelhoAsync(filter);
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
        public async Task<IActionResult> GetModeloAparelhoAsync(Guid id)
        {
            Response<ModeloAparelhoDTO> result = await _ModeloAparelhoService.GetModeloAparelhoAsync(id);
            return Ok(result);
        }

        // single by Designacao (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("designacao/{designacao}")]
        public async Task<IActionResult> GetModeloAparelhoByDesignacaoAsync(string designacao)
        {
            try
            {
                Response<ModeloAparelhoDTO> result = await _ModeloAparelhoService.GetModeloAparelhoByDesignacaoAsync(designacao);
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
        public async Task<IActionResult> CreateModeloAparelhoAsync(CreateModeloAparelhoRequest request)
        {
            try
            {
                Response<Guid> result = await _ModeloAparelhoService.CreateModeloAparelhoAsync(request);
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
        public async Task<IActionResult> UpdateModeloAparelhoAsync(UpdateModeloAparelhoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ModeloAparelhoService.UpdateModeloAparelhoAsync(request, id);
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
        public async Task<IActionResult> DeleteModeloAparelhoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ModeloAparelhoService.DeleteModeloAparelhoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleModeloAparelhoAsync([FromBody] DeleteMultipleModeloAparelhoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _ModeloAparelhoService.DeleteMultipleModeloAparelhoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
