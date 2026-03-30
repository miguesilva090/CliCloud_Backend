using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Organismos.OrganismoService;
using CliCloud.Application.Services.Organismos.OrganismoService.DTOs;
using CliCloud.Application.Services.Organismos.OrganismoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Organismos
{
    [Route("client/organismos/[controller]")]
    [ApiController]
    public class OrganismoController(IOrganismoService OrganismoService) : ControllerBase
    {
        private readonly IOrganismoService _OrganismoService = OrganismoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetOrganismoAsync(string keyword = "")
        {
            Response<IEnumerable<OrganismoDTO>> result = await _OrganismoService.GetOrganismoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetOrganismoLightAsync(string keyword = "")
        {
          Response<IEnumerable<OrganismoLightDTO>> result = await _OrganismoService.GetOrganismoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetOrganismoPaginatedAsync(OrganismoTableFilter filter)
        {
            PaginatedResponse<OrganismoTableDTO> result = await _OrganismoService.GetOrganismoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Organismos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllOrganismoAsync([FromBody] OrganismoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<OrganismoTableDTO>> result = await _OrganismoService.GetAllOrganismoAsync(filter);
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
        public async Task<IActionResult> GetOrganismoAsync(Guid id)
        {
            Response<OrganismoDTO> result = await _OrganismoService.GetOrganismoAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetOrganismoByNContribAsync(string ncontrib)
        {
          Response<OrganismoDTO> result = await _OrganismoService.GetOrganismoByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetOrganismoByNameAsync(string nome)
        {
          Response<IEnumerable<OrganismoDTO>> result = await _OrganismoService.GetOrganismoByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateOrganismoAsync(CreateOrganismoRequest request)
        {
            try
            {
                Response<Guid> result = await _OrganismoService.CreateOrganismoAsync(request);
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
        public async Task<IActionResult> UpdateOrganismoAsync(UpdateOrganismoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _OrganismoService.UpdateOrganismoAsync(request, id);
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
        public async Task<IActionResult> DeleteOrganismoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _OrganismoService.DeleteOrganismoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleOrganismoAsync([FromBody] DeleteMultipleOrganismoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _OrganismoService.DeleteMultipleOrganismoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
