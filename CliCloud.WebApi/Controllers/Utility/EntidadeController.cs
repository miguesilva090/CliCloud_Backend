using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.EntidadeService;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class EntidadeController(IEntidadeService EntidadeService) : ControllerBase
    {
        private readonly IEntidadeService _EntidadeService = EntidadeService;

        //Helper Method to normalize Image URLs
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
        private void ConvertImagemUrlToFull(EntidadeDTO dto)
        {
          if(dto == null)
          {
            return;
          }

          dto.UrlFoto = ConvertToFullUrl(dto.UrlFoto);
        }

        //Normalize Image URLs in DTO List 
        private void ConvertImagemUrlToFull(IEnumerable<EntidadeDTO> dtos)
        {
          if(dtos == null)
          {
            return;
          }

          foreach(EntidadeDTO dto in dtos)
          {
            ConvertImagemUrlToFull(dto);
          }
        }

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEntidadeAsync(string keyword = "")
        {
            Response<IEnumerable<EntidadeDTO>> result = await _EntidadeService.GetEntidadeAsync(keyword);

            if(result.Data != null)
            {
              ConvertImagemUrlToFull(result.Data);
            }
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEntidadePaginatedAsync(EntidadeTableFilter filter)
        {
            PaginatedResponse<EntidadeTableDTO> result = await _EntidadeService.GetEntidadePaginatedAsync(filter);

            // Nota: o DTO de tabela (`EntidadeTableDTO`) não contém `UrlFoto`,
            // portanto não há nada a normalizar aqui.
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntidadeAsync(Guid id)
        {
            Response<EntidadeDTO> result = await _EntidadeService.GetEntidadeAsync(id);

            if(result.Data != null)
            {
              ConvertImagemUrlToFull(result.Data);
            }
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEntidadeAsync(CreateEntidadeRequest request)
        {
            try
            {
                Response<Guid> result = await _EntidadeService.CreateEntidadeAsync(request);
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
        public async Task<IActionResult> UpdateEntidadeAsync(UpdateEntidadeRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _EntidadeService.UpdateEntidadeAsync(request, id);
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
        public async Task<IActionResult> DeleteEntidadeAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EntidadeService.DeleteEntidadeAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
