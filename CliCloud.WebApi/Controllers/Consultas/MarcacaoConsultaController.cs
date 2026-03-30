using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CliCloud.Application.Common;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Filters;
using CliCloud.Application.Services.Medicos.MedicoService;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Domain.Enums;
using System.Globalization;

namespace CliCloud.WebApi.Controllers.Consultas
{
    [Route("client/consultas/[controller]")]
    [ApiController]
    public class MarcacaoConsultaController(
        IMarcacaoConsultaService MarcacaoConsultaService,
        ICurrentTenantUserService currentTenantUserService,
        IMedicoService medicoService) : ControllerBase
    {
        private readonly IMarcacaoConsultaService _MarcacaoConsultaService = MarcacaoConsultaService;
        private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;
        private readonly IMedicoService _medicoService = medicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMarcacaoConsultaAsync(string keyword = "")
        {
            Response<IEnumerable<MarcacaoConsultaDTO>> result = await _MarcacaoConsultaService.GetMarcacaoConsultaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMarcacaoConsultaLightAsync(string keyword = "")
        {
          Response<IEnumerable<MarcacaoConsultaLightDTO>> result = await _MarcacaoConsultaService.GetMarcacaoConsultaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMarcacaoConsultaPaginatedAsync(MarcacaoConsultaTableFilter filter)
        {
            PaginatedResponse<MarcacaoConsultaTableDTO> result = await _MarcacaoConsultaService.GetMarcacaoConsultaPaginatedAsync(filter);
            return Ok(result);
        }

        // All MarcacaoConsultas (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMarcacaoConsultaAsync([FromBody] MarcacaoConsultaAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<MarcacaoConsultaTableDTO>> result = await _MarcacaoConsultaService.GetAllMarcacaoConsultaAsync(filter ?? new MarcacaoConsultaAllFilter());
            return Ok(result);
          }
          catch( Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        /// <summary>
        /// Marcações do dia. Se o utilizador tiver médico associado (Medico.IdUtilizador = UserId), filtra por esse médico;
        /// caso contrário, assume que quem está logado é médico e devolve todas as marcações do dia (até haver distinção de perfis).
        /// </summary>
        [Authorize(Roles = "client")]
        [HttpGet("consultas-do-dia")]
        public async Task<IActionResult> GetConsultasDoDiaMedicoLogadoAsync([FromQuery] DateTime? data = null)
        {
            var dataConsulta = (data ?? DateTime.UtcNow.Date).Date;
            var dataStr = dataConsulta.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var filters = new List<TableFilter>
            {
                new TableFilter { Id = "data", Value = dataStr }
            };

            string? userIdStr = _currentTenantUserService.UserId;
            if (!string.IsNullOrWhiteSpace(userIdStr) && Guid.TryParse(userIdStr, out Guid userId))
            {
                var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
                if (medicoRes.Status == ResponseStatus.Success && medicoRes.Data != null)
                    filters.Add(new TableFilter { Id = "medicoid", Value = medicoRes.Data.Id.ToString() });
            }

            var filter = new MarcacaoConsultaAllFilter
            {
                Filters = filters,
                Sorting = [new TanstackColumnOrder { Id = "data", Desc = false }, new TanstackColumnOrder { Id = "horaMarcacao", Desc = false }]
            };
            Response<IEnumerable<MarcacaoConsultaTableDTO>> result = await _MarcacaoConsultaService.GetAllMarcacaoConsultaAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("status-consulta-options")]
        public IActionResult GetStatusConsultaOptions()
        {
            var options = Enum.GetValues<StatusConsulta>()
                .Select(e => new { value = (int)e, label = EnumDisplayHelper.GetDisplayName(e) })
                .ToList();
            return Ok(ResponseFactory.Success(options));
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarcacaoConsultaAsync(Guid id)
        {
            Response<MarcacaoConsultaDTO> result = await _MarcacaoConsultaService.GetMarcacaoConsultaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMarcacaoConsultaAsync(CreateMarcacaoConsultaRequest request)
        {
            try
            {
                Response<Guid> result = await _MarcacaoConsultaService.CreateMarcacaoConsultaAsync(request);
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
        public async Task<IActionResult> UpdateMarcacaoConsultaAsync(UpdateMarcacaoConsultaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _MarcacaoConsultaService.UpdateMarcacaoConsultaAsync(request, id);
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
        public async Task<IActionResult> DeleteMarcacaoConsultaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MarcacaoConsultaService.DeleteMarcacaoConsultaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleMarcacaoConsultaAsync([FromBody] DeleteMultipleMarcacaoConsultaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _MarcacaoConsultaService.DeleteMultipleMarcacaoConsultaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
