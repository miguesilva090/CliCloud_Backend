using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService;
using CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class ModelosRelatorioAtestadoController(
        IModeloRelatorioAtestadoService service,
        ICurrentTenantUserService currentUser) : ControllerBase
    {
        private readonly IModeloRelatorioAtestadoService _service = service;
        private readonly ICurrentTenantUserService _currentUser = currentUser;

        // Lista de modelos para a empresa atual (e médico atual, se existir)
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // TODO: quando existir suporte a multi-empresa no CurrentTenantUserService,
            // substituir Guid.Empty pelo identificador real da empresa/tenant.
            var empresaId = Guid.Empty;

            Guid? medicoId = null;
            if (Guid.TryParse(_currentUser.UserId, out var medId))
                medicoId = medId;

            Response<IEnumerable<ModeloRelatorioAtestadoDTO>> result =
                await _service.GetModelosAsync(empresaId, medicoId);

            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateModeloRelatorioAtestadoRequest request)
        {
            var empresaId = Guid.Empty;
            var result = await _service.CreateAsync(request, empresaId);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateModeloRelatorioAtestadoRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }
    }
}

