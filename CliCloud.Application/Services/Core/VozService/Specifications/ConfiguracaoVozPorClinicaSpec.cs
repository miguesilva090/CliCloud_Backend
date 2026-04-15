using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.VozService.Specifications
{
  public class ConfiguracaoVozPorClinicaSpec : Specification<ConfiguracaoVoz>
  {
    public ConfiguracaoVozPorClinicaSpec(Guid clinicaId)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
  }
}
