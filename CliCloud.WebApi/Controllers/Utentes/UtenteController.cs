using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utentes.UtenteService;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utentes
{
    [Route("client/utentes/[controller]")]
    [ApiController]
    public class UtenteController(IUtenteService UtenteService) : ControllerBase
    {
        private readonly IUtenteService _UtenteService = UtenteService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetUtenteAsync(string keyword = "")
        {
            Response<IEnumerable<UtenteDTO>> result = await _UtenteService.GetUtenteAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetUtenteLightAsync(string keyword = "")
        {
          Response<IEnumerable<UtenteLightDTO>> result = await _UtenteService.GetUtenteLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetUtentePaginatedAsync(UtenteTableFilter filter)
        {
            PaginatedResponse<UtenteTableDTO> result = await _UtenteService.GetUtentePaginatedAsync(filter);
            return Ok(result);
        }

        // All Utentes (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllUtenteAsync([FromBody] UtenteAllFilter filter)
        {
          try
          {
            Response<IEnumerable<UtenteTableDTO>> result = await _UtenteService.GetAllUtenteAsync(filter);
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
        public async Task<IActionResult> GetUtenteAsync(Guid id)
        {
            Response<UtenteDTO> result = await _UtenteService.GetUtenteAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetUtenteByNContribAsync(string ncontrib)
        {
          Response<UtenteDTO> result = await _UtenteService.GetUtenteByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetUtenteByNameAsync(string nome)
        {
          Response<IEnumerable<UtenteDTO>> result = await _UtenteService.GetUtenteByNameAsync(nome);
          return Ok(result);
        }

        // Single by NumeroUtente 
        [Authorize(Roles = "client")]
        [HttpGet("numeroUtente/{numeroUtente}")]
        public async Task<IActionResult> GetUtenteByNumeroUtenteAsync(string numeroUtente)
        {
          Response<UtenteDTO> result = await _UtenteService.GetUtenteByNumeroUtenteAsync(numeroUtente);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateUtenteAsync(CreateUtenteRequest request)
        {
            try
            {
                Response<Guid> result = await _UtenteService.CreateUtenteAsync(request);
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
        public async Task<IActionResult> UpdateUtenteAsync(UpdateUtenteRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _UtenteService.UpdateUtenteAsync(request, id);
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
        public async Task<IActionResult> DeleteUtenteAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _UtenteService.DeleteUtenteAsync(id);
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
        public async Task<IActionResult> DeleteMultipleUtenteAsync([FromBody] DeleteMultipleUtenteRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _UtenteService.DeleteMultipleUtenteAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
