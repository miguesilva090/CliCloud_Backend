using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.Specifications
{
  public class TeleconsultaPorClinicaMarcacaoSpec : Specification<TeleconsultaSessao>
  {
    public TeleconsultaPorClinicaMarcacaoSpec(Guid clinicaId, Guid consultaMarcacaoId)
    {
      _ = Query.Where(x => x.ClinicaId == clinicaId && x.ConsultaMarcacaoId == consultaMarcacaoId);
    }
  }
}
