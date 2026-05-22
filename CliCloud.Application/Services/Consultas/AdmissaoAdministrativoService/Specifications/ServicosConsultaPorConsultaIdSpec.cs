using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class ServicosConsultaPorConsultaIdSpec : Specification<ServicoConsulta>
{
  public ServicosConsultaPorConsultaIdSpec(Guid consultaId)
  {
    _ = Query.Where(s => s.ConsultaId == consultaId && s.DeletedOn == null);
  }
}
