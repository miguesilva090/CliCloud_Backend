using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.Specifications
{
  public class TeleconsultaPorClinicaSessaoSpec : Specification<TeleconsultaSessao>
  {
    public TeleconsultaPorClinicaSessaoSpec(Guid clinicaId, Guid sessaoId)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId && x.Id == sessaoId);
    }
  }
}
