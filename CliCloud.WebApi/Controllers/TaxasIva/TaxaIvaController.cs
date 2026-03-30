using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs;
using CliCloud.Application.Services.TaxasIva.TaxaIvaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.TaxasIva
{
    [Route("client/taxas-iva/[controller]")]
    [ApiController]
    public class TaxaIvaController(ITaxaIvaService taxaIvaService) : ControllerBase
    {
        private readonly ITaxaIvaService _taxaIvaService = taxaIvaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTaxaIvaAsync(string keyword = "")
        {
            Response<IEnumerable<TaxaIvaDTO>> result = await _taxaIvaService.GetTaxaIvaAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTaxaIvaLightAsync(string keyword = "")
        {
            Response<IEnumerable<TaxaIvaLightDTO>> result = await _taxaIvaService.GetTaxaIvaLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTaxaIvaPaginatedAsync(TaxaIvaTableFilter filter)
        {
            PaginatedResponse<TaxaIvaTableDTO> result = await _taxaIvaService.GetTaxaIvaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTaxaIvaAsync([FromBody] TaxaIvaAllFilter filter)
        {
            try
            {
                Response<IEnumerable<TaxaIvaTableDTO>> result = await _taxaIvaService.GetAllTaxaIvaAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaxaIvaAsync(Guid id)
        {
            Response<TaxaIvaDTO> result = await _taxaIvaService.GetTaxaIvaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTaxaIvaAsync(CreateTaxaIvaRequest request)
        {
            try
            {
                Response<Guid> result = await _taxaIvaService.CreateTaxaIvaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaxaIvaAsync([FromRoute] Guid id, [FromBody] UpdateTaxaIvaRequest request)
        {
            try
            {
                Response<Guid> result = await _taxaIvaService.UpdateTaxaIvaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxaIvaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _taxaIvaService.DeleteTaxaIvaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleTaxaIvaAsync([FromBody] DeleteMultipleTaxaIvaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _taxaIvaService.DeleteMultipleTaxaIvaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
