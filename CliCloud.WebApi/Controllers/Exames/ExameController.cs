using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Exames.ExameService;
using CliCloud.Application.Services.Exames.ExameService.DTOs;
using CliCloud.Application.Services.Exames.ExameService.Filters;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Medicos.MedicoService;

namespace CliCloud.WebApi.Controllers.Exames
{
    [Route("client/exames/[controller]")]
    [ApiController]
  public class ExameController(
    IExameService exameService,
    ICurrentTenantUserService currentTenantUserService,
    IMedicoService medicoService
  ) : ControllerBase
    {
    private readonly IExameService _exameService = exameService;
    private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;
    private readonly IMedicoService _medicoService = medicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetExameAsync(string keyword = "")
        {
            Response<IEnumerable<ExameDTO>> result = await _exameService.GetExameAsync(keyword);
            return Ok(result);
        }

        //Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetExameLightAsync(string keyword = "")
        {
            Response<IEnumerable<ExameLightDTO>> result = await _exameService.GetExameLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetExamePaginatedAsync(ExameTableFilter filter)
        {
            PaginatedResponse<ExameTableDTO> result = await _exameService.GetExamePaginatedAsync(filter);
            return Ok(result);
        }

        // All Exames (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllExameAsync([FromBody] ExameAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<ExameTableDTO>> result = await _exameService.GetAllExameAsync(filter ?? new ExameAllFilter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExameAsync(Guid id)
        {
            Response<ExameDTO> result = await _exameService.GetExameAsync(id);
            return Ok(result);
        }

        // resultados de exames por prescrição (ExameId)
        [Authorize(Roles = "client")]
        [HttpGet("{id}/resultados")]
        public async Task<IActionResult> GetResultadosByExameAsync(Guid id)
        {
            Response<IEnumerable<ResultadoExameTableDTO>> result = await _exameService.GetResultadosByExameAsync(id);
            return Ok(result);
        }

        // dados para relatório de prescrição de exames
        [Authorize(Roles = "client")]
        [HttpGet("{id}/report")]
        public async Task<IActionResult> GetExameReportAsync(Guid id)
        {
            Response<ExamePrescricaoReportDTO> result = await _exameService.GetExameReportAsync(id);
            return Ok(result);
        }

        // upsert de resultado para uma linha específica da prescrição
        public record UpsertResultadoLinhaRequest(Guid LinhaId, string? Valor, string? Referencia, string? Obs);

        [Authorize(Roles = "client")]
        [HttpPost("{id}/resultados")]
        public async Task<IActionResult> UpsertResultadoLinhaAsync(Guid id, [FromBody] UpsertResultadoLinhaRequest request)
        {
            Response<Guid> result = await _exameService.UpsertResultadoLinhaAsync(id, request.LinhaId, request.Valor, request.Referencia, request.Obs);
            return Ok(result);
        }

        // create
    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateExameAsync(CreateExameRequest request)
    {
      try
      {
        // Garantir que o MedicoId corresponde ao médico associado ao utilizador logado
        string? userIdStr = _currentTenantUserService.UserId;
        if (!string.IsNullOrWhiteSpace(userIdStr) && Guid.TryParse(userIdStr, out Guid userId))
        {
          var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
          if (medicoRes.Status == ResponseStatus.Success && medicoRes.Data != null)
          {
            request.MedicoId = medicoRes.Data.Id;
          }
        }

        Response<Guid> result = await _exameService.CreateExameAsync(request);
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
    public async Task<IActionResult> UpdateExameAsync(UpdateExameRequest request, Guid id)
    {
      try
      {
        // Reaproveitar a mesma lógica para garantir MedicoId consistente
        string? userIdStr = _currentTenantUserService.UserId;
        if (!string.IsNullOrWhiteSpace(userIdStr) && Guid.TryParse(userIdStr, out Guid userId))
        {
          var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
          if (medicoRes.Status == ResponseStatus.Success && medicoRes.Data != null)
          {
            request.MedicoId = medicoRes.Data.Id;
          }
        }

        Response<Guid> result = await _exameService.UpdateExameAsync(request, id);
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
        public async Task<IActionResult> DeleteExameAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _exameService.DeleteExameAsync(id);
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
        public async Task<IActionResult> DeleteMultipleExameAsync([FromBody] DeleteMultipleExameRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> response = await _exameService.DeleteMultipleExameAsync(request.Ids);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
