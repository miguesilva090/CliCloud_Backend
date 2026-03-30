using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Tratamentos;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  public class ConfiguracaoTratamentosPorClinicaSpec : Specification<ConfiguracaoTratamentos>
  {
    public ConfiguracaoTratamentosPorClinicaSpec(Guid clinicaId)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
  }
}

