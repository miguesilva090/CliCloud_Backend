using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissoesParaFechoSpec : Specification<Admissao>
{
  public AdmissoesParaFechoSpec(DateTime data)
  {
    DateTime dia = data.Date;
    _ = Query
      .Where(x => x.Data.HasValue && x.Data.Value.Date == dia)
      .Include(x => x.Servicos)
      .Include(x => x.TipoAdmissao);
  }
}
