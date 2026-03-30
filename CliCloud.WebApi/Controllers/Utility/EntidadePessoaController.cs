using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.EntidadePessoaService;
using CliCloud.Application.Services.Utility.EntidadePessoaService.DTOs;
using CliCloud.Application.Services.Utility.EntidadePessoaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class EntidadePessoaController( IEntidadePessoaService EntidadePessoaService) : ControllerBase
    {
        private readonly IEntidadePessoaService _EntidadePessoaService = EntidadePessoaService;

        // Helper Method to normalize Image URLs
        private string? ConvertToFullUrl(string? partialUrl)
        {
          if(string.IsNullOrWhiteSpace(partialUrl))
          {
            return null;
          }

          if(partialUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
          || partialUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
          {
            return partialUrl;
          }

          string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
          
          if(!partialUrl.StartsWith('/'))
          {
            partialUrl = "/" + partialUrl;
          }

          return $"{baseUrl}{partialUrl}";
        }

        //Normalize Image URLs in DTO 
        private void ConvertImagemUrlToFull(EntidadePessoaDTO dto)
        {
          if(dto == null)
          {
            return;
          }
          dto.UrlFoto = ConvertToFullUrl(dto.UrlFoto);
          dto.UrlFotoAssinatura = ConvertToFullUrl(dto.UrlFotoAssinatura);
        }

        //Normalize Image URLs in DTO List
        private void ConvertImagemUrlToFull(IEnumerable<EntidadePessoaDTO> dtos)
        {
          if(dtos == null)
          {
            return;
          }

          foreach(EntidadePessoaDTO dto in dtos)
          {
            ConvertImagemUrlToFull(dto);
          }
        }

        // Normalize Image URLs on table DTO
        private void ConvertImagemUrlToFull(EntidadePessoaTableDTO dto)
        {
          if(dto == null)
          {
            return;
          }

          dto.UrlFotoAssinatura = ConvertToFullUrl(dto.UrlFotoAssinatura);
        }

        // Normalize Image URLs on table DTO List 
        private void ConvertImagemUrlToFull(IEnumerable<EntidadePessoaTableDTO> dtos)
        {
          if(dtos == null)
          {
            return;
          }

          foreach(EntidadePessoaTableDTO dto in dtos)
          {
            ConvertImagemUrlToFull(dto);
          }
        }
        

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEntidadePessoaAsync(string keyword = "")
        {
            Response<IEnumerable<EntidadePessoaDTO>> result = await _EntidadePessoaService.GetEntidadePessoaAsync(keyword);
            
            if(result.Data != null)
            {
              ConvertImagemUrlToFull(result.Data);
            }

          return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetEntidadePessoaLightAsync(string keyword = "")
        {
          Response<IEnumerable<EntidadePessoaLightDTO>> result = await _EntidadePessoaService.GetEntidadePessoaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEntidadePessoaPaginatedAsync(EntidadePessoaTableFilter filter)
        {
            PaginatedResponse<EntidadePessoaTableDTO> result = await _EntidadePessoaService.GetEntidadePessoaPaginatedAsync(filter);
            if(result.Data != null)
            {
              ConvertImagemUrlToFull(result.Data);
            }

            return Ok(result);
        }

        // Full List
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEntidadePessoaAsync([FromBody]EntidadePessoaAllFilter? filter = null) 
        {
          try
          {
            Response<IEnumerable<EntidadePessoaTableDTO>> result = await _EntidadePessoaService.GetAllEntidadePessoaAsync(filter ?? new EntidadePessoaAllFilter());

            if(result.Data != null)
            {
              ConvertImagemUrlToFull(result.Data);
            }

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
        public async Task<IActionResult> GetEntidadePessoaAsync(Guid id)
        {
            Response<EntidadePessoaDTO> result = await _EntidadePessoaService.GetEntidadePessoaAsync(id);
            if(result.Data != null)
            {
              ConvertImagemUrlToFull(result.Data);
            }

            return Ok(result);
        }

        //Single by NContrib
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetEntidadePessoaByNContribAsync(string ncontrib)
        {
          Response<EntidadePessoaDTO> result = await _EntidadePessoaService.GetEntidadePessoaByNContribAsync(ncontrib);

          if(result.Data != null)
          {
            ConvertImagemUrlToFull(result.Data);
          }

          return Ok(result);
        }

        //Single by Nome
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetEntidadePessoaByNameAsync(string nome)
        {
          Response<IEnumerable<EntidadePessoaDTO>> result = await _EntidadePessoaService.GetEntidadePessoaByNameAsync(nome);

          if(result.Data != null)
          {
            ConvertImagemUrlToFull(result.Data);
          }

          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEntidadePessoaAsync(CreateEntidadePessoaRequest request)
        {
            try
            {
                Response<Guid> result = await _EntidadePessoaService.CreateEntidadePessoaAsync(request);
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
        public async Task<IActionResult> UpdateEntidadePessoaAsync(UpdateEntidadePessoaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _EntidadePessoaService.UpdateEntidadePessoaAsync(request, id);
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
        public async Task<IActionResult> DeleteEntidadePessoaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EntidadePessoaService.DeleteEntidadePessoaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete Multiple Bulk 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleEntidadePessoaAsync([FromBody] DeleteMultipleEntidadePessoaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _EntidadePessoaService.DeleteMultipleEntidadePessoaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
