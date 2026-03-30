using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.DTOs;

namespace CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService
{
  public interface IUnidadesLocaisSaudeService : ITransientService
  {
    Task<Response<IEnumerable<UnidadesLocaisSaudeLightDTO>>> GetUnidadesLocaisSaudeLightAsync(string keyword = "");
  }
}

