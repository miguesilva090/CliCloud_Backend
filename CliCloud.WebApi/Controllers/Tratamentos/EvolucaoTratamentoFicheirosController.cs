using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
  [Route("client/tratamentos/[controller]")]
  [ApiController]
  public class EvolucaoTratamentoFicheirosController(
    IEvolucaoTratamentoFicheiroService service
  ) : ControllerBase
  {
    private readonly IEvolucaoTratamentoFicheiroService _service = service;

    // GET: lista de ficheiros de uma evolução específica
    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetByEvolucaoTratamentoIdAsync([FromQuery] Guid evolucaoTratamentoId)
    {
      Response<IEnumerable<EvolucaoTratamentoFicheiroDTO>> result =
        await _service.GetByEvolucaoTratamentoIdAsync(evolucaoTratamentoId);
      return Ok(result);
    }

    // POST: criar novo registo de ficheiro (metadados)
    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateEvolucaoTratamentoFicheiroRequest request)
    {
      Response<Guid> result = await _service.CreateAsync(request);
      return Ok(result);
    }

    // DELETE: remover um ficheiro
    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      Response<Guid> result = await _service.DeleteAsync(id);
      return Ok(result);
    }
  }
}

