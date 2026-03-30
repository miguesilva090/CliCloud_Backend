using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Medicos.MedicoService;
using CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Medicos
{
    [Route("client/medicos/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _MedicoService;
        private readonly ICurrentTenantUserService _currentUser;

        public MedicoController(IMedicoService medicoService, ICurrentTenantUserService currentUser)
        {
            _MedicoService = medicoService;
            _currentUser = currentUser;
        }

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMedicoAsync(string keyword = "")
        {
            Response<IEnumerable<MedicoDTO>> result = await _MedicoService.GetMedicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMedicoLightAsync(string keyword = "")
        {
          Response<IEnumerable<MedicoLightDTO>> result = await _MedicoService.GetMedicoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMedicoPaginatedAsync(MedicoTableFilter filter)
        {
            PaginatedResponse<MedicoTableDTO> result = await _MedicoService.GetMedicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Medicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMedicoAsync([FromBody] MedicoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<MedicoTableDTO>> result = await _MedicoService.GetAllMedicoAsync(filter);
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
        public async Task<IActionResult> GetMedicoAsync(Guid id)
        {
            Response<MedicoDTO> result = await _MedicoService.GetMedicoAsync(id);
            return Ok(result);
        }

        // current medico by logged-in user
        [Authorize(Roles = "client")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentMedicoAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentUser.UserId) || !Guid.TryParse(_currentUser.UserId, out Guid userId))
            {
                return BadRequest("Utilizador atual inválido.");
            }

            Response<MedicoDTO?> result = await _MedicoService.GetMedicoByIdUtilizadorAsync(userId);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetMedicoByNContribAsync(string ncontrib)
        {
          Response<MedicoDTO> result = await _MedicoService.GetMedicoByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetMedicoByNameAsync(string nome)
        {
          Response<IEnumerable<MedicoDTO>> result = await _MedicoService.GetMedicoByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicoAsync(CreateMedicoRequest request)
        {
            try
            {
                Response<Guid> result = await _MedicoService.CreateMedicoAsync(request);
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
        public async Task<IActionResult> UpdateMedicoAsync([FromRoute] Guid id, [FromBody] UpdateMedicoRequest request)
        {
            try
            {
                Response<Guid> result = await _MedicoService.UpdateMedicoAsync(request, id);
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
        public async Task<IActionResult> DeleteMedicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MedicoService.DeleteMedicoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleMedicoAsync([FromBody] DeleteMultipleMedicoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _MedicoService.DeleteMultipleMedicoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
