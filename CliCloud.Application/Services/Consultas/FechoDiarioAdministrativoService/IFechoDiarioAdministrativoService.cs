using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;

public interface IFechoDiarioAdministrativoService : ITransientService
{
  Task<Response<FechoDiarioResultDTO>> ExecutarFechoAsync(FechoDiarioRequest request);
}
