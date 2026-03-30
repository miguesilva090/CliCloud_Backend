using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.FornecedoresService.FornecedorService;
using CliCloud.Application.Services.FornecedoresService.FornecedorService.DTOs;
using CliCloud.Application.Services.FornecedoresService.FornecedorService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Fornecedores
{
    [Route("client/fornecedores/[controller]")]
    [ApiController]
    public class FornecedorController(IFornecedorService FornecedorService) : ControllerBase
    {
        private readonly IFornecedorService _FornecedorService = FornecedorService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetFornecedorAsync(string keyword = "")
        {
            Response<IEnumerable<FornecedorDTO>> result = await _FornecedorService.GetFornecedorAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetFornecedorLightAsync(string keyword = "")
        {
          Response<IEnumerable<FornecedorLightDTO>> result = await _FornecedorService.GetFornecedorLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetFornecedorPaginatedAsync(FornecedorTableFilter filter)
        {
            PaginatedResponse<FornecedorTableDTO> result = await _FornecedorService.GetFornecedorPaginatedAsync(filter);
            return Ok(result);
        }

        // All Fornecedores (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllFornecedorAsync([FromBody] FornecedorAllFilter filter)
        {
          try
          {
            Response<IEnumerable<FornecedorTableDTO>> result = await _FornecedorService.GetAllFornecedorAsync(filter);
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
        public async Task<IActionResult> GetFornecedorAsync(Guid id)
        {
            Response<FornecedorDTO> result = await _FornecedorService.GetFornecedorAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetFornecedorByNContribAsync(string ncontrib)
        {
          Response<FornecedorDTO> result = await _FornecedorService.GetFornecedorByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetFornecedorByNameAsync(string nome)
        {
          Response<IEnumerable<FornecedorDTO>> result = await _FornecedorService.GetFornecedorByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateFornecedorAsync(CreateFornecedorRequest request)
        {
            try
            {
                Response<Guid> result = await _FornecedorService.CreateFornecedorAsync(request);
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
        public async Task<IActionResult> UpdateFornecedorAsync(UpdateFornecedorRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _FornecedorService.UpdateFornecedorAsync(request, id);
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
        public async Task<IActionResult> DeleteFornecedorAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _FornecedorService.DeleteFornecedorAsync(id);
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
        public async Task<IActionResult> DeleteMultipleFornecedorAsync([FromBody] DeleteMultipleFornecedorRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _FornecedorService.DeleteMultipleFornecedorAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
