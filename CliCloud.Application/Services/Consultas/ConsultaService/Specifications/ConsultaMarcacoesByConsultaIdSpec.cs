using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications;

public sealed class ConsultaMarcacoesByConsultaIdSpec : Specification<ConsultaMarcacao>
{
  public ConsultaMarcacoesByConsultaIdSpec(Guid consultaId)
  {
    _ = Query.Where(x => x.ConsultaId == consultaId && x.DeletedOn == null);
  }
}
