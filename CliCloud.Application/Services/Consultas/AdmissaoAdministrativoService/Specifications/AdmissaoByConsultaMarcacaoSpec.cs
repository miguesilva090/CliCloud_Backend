using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissaoByConsultaMarcacaoSpec : Specification<Admissao>
{
  public AdmissaoByConsultaMarcacaoSpec(Guid consultaMarcacaoId)
  {
    _ = Query.Where(x =>
      x.ConsultaMarcacaoId == consultaMarcacaoId && x.DeletedOn == null
    );
  }
}
