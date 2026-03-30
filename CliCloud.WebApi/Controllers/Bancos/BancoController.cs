using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Bancos.BancoService;
using CliCloud.Application.Services.Bancos.BancoService.DTOs;
using CliCloud.Application.Services.Bancos.BancoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Bancos
{
    [Route("client/bancos/[controller]")]
    [ApiController]
    public class BancoController(IBancoService BancoService) : ControllerBase
    {
        private readonly IBancoService _BancoService = BancoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetBancoAsync(string keyword = "")
        {
            Response<IEnumerable<BancoDTO>> result = await _BancoService.GetBancoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetBancoLightAsync(string keyword = "")
        {
          Response<IEnumerable<BancoLightDTO>> result = await _BancoService.GetBancoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetBancoPaginatedAsync(BancoTableFilter filter)
        {
            PaginatedResponse<BancoTableDTO> result = await _BancoService.GetBancoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Bancos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllBancoAsync([FromBody] BancoAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<BancoTableDTO>> result = await _BancoService.GetAllBancoAsync(filter ?? new BancoAllFilter());
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
        public async Task<IActionResult> GetBancoAsync(Guid id)
        {
            Response<BancoDTO> result = await _BancoService.GetBancoAsync(id);
            return Ok(result);
        }

        //Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetBancoByNContribAsync(string ncontrib)
        {
          Response<BancoDTO> result = await _BancoService.GetBancoByNContribAsync(ncontrib);
          return Ok(result);
        }

        //Get multiple by Nome
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetBancoByNameAsync(string nome)
        {
          Response<IEnumerable<BancoDTO>> result = await _BancoService.GetBancoByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateBancoAsync(CreateBancoRequest request)
        {
            try
            {
                Response<Guid> result = await _BancoService.CreateBancoAsync(request);
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
        public async Task<IActionResult> UpdateBancoAsync(UpdateBancoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _BancoService.UpdateBancoAsync(request, id);
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
        public async Task<IActionResult> DeleteBancoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _BancoService.DeleteBancoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleBancoAsync([FromBody] DeleteMultipleBancoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _BancoService.DeleteMultipleBancoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
