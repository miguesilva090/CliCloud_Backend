using System.Collections.Generic;
using System.Threading.Tasks;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.TipoAdmissaoService.DTOs;

namespace CliCloud.Application.Services.Consultas.TipoAdmissaoService
{
  public interface ITipoAdmissaoService : ITransientService
  {
    Task<Response<IEnumerable<TipoAdmissaoDTO>>> GetAllAsync();
  }
}

