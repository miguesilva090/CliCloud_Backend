using System.Collections.Generic;
using System.Threading.Tasks;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService
{
  public interface IMotivoConsultaService : ITransientService
  {
    Task<Response<IEnumerable<MotivoConsultaDTO>>> GetAllAsync();
  }
}
