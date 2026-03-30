using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Medicos.MedicoExternoService;
using CliCloud.Application.Services.Medicos.MedicoExternoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoExternoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Medicos
{
    [Route("client/medicos/[controller]")]
    [ApiController]
    public class MedicoExternoController(IMedicoExternoService MedicoExternoService) : ControllerBase
    {
        private readonly IMedicoExternoService _MedicoExternoService = MedicoExternoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMedicoExternoAsync(string keyword = "")
        {
            Response<IEnumerable<MedicoExternoDTO>> result = await _MedicoExternoService.GetMedicoExternoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMedicoExternoLightAsync(string keyword = "")
        {
          Response<IEnumerable<MedicoExternoLightDTO>> result = await _MedicoExternoService.GetMedicoExternoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMedicoExternoPaginatedAsync(MedicoExternoTableFilter filter)
        {
            PaginatedResponse<MedicoExternoTableDTO> result = await _MedicoExternoService.GetMedicoExternoPaginatedAsync(filter);
            return Ok(result);
        }

        // All MedicosExternos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMedicoExternoAsync([FromBody] MedicoExternoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<MedicoExternoTableDTO>> result = await _MedicoExternoService.GetAllMedicoExternoAsync(filter);
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
        public async Task<IActionResult> GetMedicoExternoAsync(Guid id)
        {
            Response<MedicoExternoDTO> result = await _MedicoExternoService.GetMedicoExternoAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetMedicoExternoByNContribAsync(string ncontrib)
        {
          Response<MedicoExternoDTO> result = await _MedicoExternoService.GetMedicoExternoByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetMedicoExternoByNameAsync(string nome)
        {
          Response<IEnumerable<MedicoExternoDTO>> result = await _MedicoExternoService.GetMedicoExternoByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicoExternoAsync(CreateMedicoExternoRequest request)
        {
            try
            {
                Response<Guid> result = await _MedicoExternoService.CreateMedicoExternoAsync(request);
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
        public async Task<IActionResult> UpdateMedicoExternoAsync(UpdateMedicoExternoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _MedicoExternoService.UpdateMedicoExternoAsync(request, id);
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
        public async Task<IActionResult> DeleteMedicoExternoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MedicoExternoService.DeleteMedicoExternoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleMedicoExternoAsync([FromBody] DeleteMultipleMedicoExternoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _MedicoExternoService.DeleteMultipleMedicoExternoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
