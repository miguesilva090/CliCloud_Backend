using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Empresas.EmpresaService;
using CliCloud.Application.Services.Empresas.EmpresaService.DTOs;
using CliCloud.Application.Services.Empresas.EmpresaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Empresas
{
    [Route("client/empresas/[controller]")]
    [ApiController]
    public class EmpresaController(IEmpresaService empresaService) : ControllerBase
    {
        private readonly IEmpresaService _empresaService = empresaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEmpresaAsync(string keyword = "")
        {
            Response<IEnumerable<EmpresaDTO>> result = await _empresaService.GetEmpresaAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEmpresaPaginatedAsync(EmpresaTableFilter filter)
        {
            PaginatedResponse<EmpresaTableDTO> result = await _empresaService.GetEmpresaPaginatedAsync(filter);
            return Ok(result);
        }

        // All Empresas (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEmpresaAsync([FromBody] EmpresaAllFilter filter)
        {
          try
          {
            Response<IEnumerable<EmpresaTableDTO>> result = await _empresaService.GetAllEmpresaAsync(filter);
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
        public async Task<IActionResult> GetEmpresaAsync(Guid id)
        {
            Response<EmpresaDTO> result = await _empresaService.GetEmpresaAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetEmpresaByNContribAsync(string ncontrib)
        {
          Response<EmpresaDTO> result = await _empresaService.GetEmpresaByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetEmpresaByNameAsync(string nome)
        {
          Response<IEnumerable<EmpresaDTO>> result = await _empresaService.GetEmpresaByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEmpresaAsync(CreateEmpresaRequest request)
        {
            try
            {
                Response<Guid> result = await _empresaService.CreateEmpresaAsync(request);
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
        public async Task<IActionResult> UpdateEmpresaAsync(UpdateEmpresaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _empresaService.UpdateEmpresaAsync(request, id);
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
        public async Task<IActionResult> DeleteEmpresaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _empresaService.DeleteEmpresaAsync(id);
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
        public async Task<IActionResult> DeleteMultipleEmpresaAsync([FromBody] DeleteMultipleEmpresaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _empresaService.DeleteMultipleEmpresaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}

