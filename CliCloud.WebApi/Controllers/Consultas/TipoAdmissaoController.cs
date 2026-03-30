using System.Threading.Tasks;
using CliCloud.Application.Services.Consultas.TipoAdmissaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas
{
  [Route("client/consultas/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class TipoAdmissaoController : ControllerBase
  {
    private readonly ITipoAdmissaoService _tipoAdmissaoService;

    public TipoAdmissaoController(ITipoAdmissaoService tipoAdmissaoService)
    {
      _tipoAdmissaoService = tipoAdmissaoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var result = await _tipoAdmissaoService.GetAllAsync();
      return Ok(result);
    }
  }
}

