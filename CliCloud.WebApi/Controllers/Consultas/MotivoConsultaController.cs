using System.Threading.Tasks;
using CliCloud.Application.Services.Consultas.MotivoConsultaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas
{
  [Route("client/consultas/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class MotivoConsultaController : ControllerBase
  {
    private readonly IMotivoConsultaService _motivoConsultaService;

    public MotivoConsultaController(IMotivoConsultaService motivoConsultaService)
    {
      _motivoConsultaService = motivoConsultaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var result = await _motivoConsultaService.GetAllAsync();
      return Ok(result);
    }
  }
}
