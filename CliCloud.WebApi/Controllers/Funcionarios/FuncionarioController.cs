using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Funcionarios.FuncionarioService;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.DTOs;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Funcionarios
{
    [Route("client/funcionarios/[controller]")]
    [ApiController]
    public class FuncionarioController(IFuncionarioService FuncionarioService) : ControllerBase
    {
        private readonly IFuncionarioService _FuncionarioService = FuncionarioService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetFuncionarioAsync(string keyword = "")
        {
            Response<IEnumerable<FuncionarioDTO>> result = await _FuncionarioService.GetFuncionarioAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetFuncionarioLightAsync(string keyword = "")
        {
          Response<IEnumerable<FuncionarioLightDTO>> result = await _FuncionarioService.GetFuncionarioLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetFuncionarioPaginatedAsync(FuncionarioTableFilter filter)
        {
            PaginatedResponse<FuncionarioTableDTO> result = await _FuncionarioService.GetFuncionarioPaginatedAsync(filter);
            return Ok(result);
        }

        // All Funcionarios (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllFuncionarioAsync([FromBody] FuncionarioAllFilter filter)
        {
          try
          {
            Response<IEnumerable<FuncionarioTableDTO>> result = await _FuncionarioService.GetAllFuncionarioAsync(filter);
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
        public async Task<IActionResult> GetFuncionarioAsync(Guid id)
        {
            Response<FuncionarioDTO> result = await _FuncionarioService.GetFuncionarioAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetFuncionarioByNContribAsync(string ncontrib)
        {
          Response<FuncionarioDTO> result = await _FuncionarioService.GetFuncionarioByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetFuncionarioByNameAsync(string nome)
        {
          Response<IEnumerable<FuncionarioDTO>> result = await _FuncionarioService.GetFuncionarioByNameAsync(nome);
          return Ok(result);
        }


        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateFuncionarioAsync(CreateFuncionarioRequest request)
        {
            try
            {
                Response<Guid> result = await _FuncionarioService.CreateFuncionarioAsync(request);
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
        public async Task<IActionResult> UpdateFuncionarioAsync(UpdateFuncionarioRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _FuncionarioService.UpdateFuncionarioAsync(request, id);
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
        public async Task<IActionResult> DeleteFuncionarioAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _FuncionarioService.DeleteFuncionarioAsync(id);
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
        public async Task<IActionResult> DeleteMultipleFuncionarioAsync([FromBody] DeleteMultipleFuncionarioRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _FuncionarioService.DeleteMultipleFuncionarioAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
