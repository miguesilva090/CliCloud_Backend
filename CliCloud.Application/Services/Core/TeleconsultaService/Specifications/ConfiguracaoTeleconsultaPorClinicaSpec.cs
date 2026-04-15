using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.TeleconsultaService.Specifications
{
  public class ConfiguracaoTeleconsultaPorClinicaSpec : Specification<ConfiguracaoTeleconsulta>
  {
    public ConfiguracaoTeleconsultaPorClinicaSpec(Guid clinicaId)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
  }
}
