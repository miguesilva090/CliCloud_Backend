using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

/// <summary>
/// Usa <see cref="EF.Constant{T}(T)"/> para o SQL Server gerar <c>IN (...)</c> em vez de
/// <c>OPENJSON(...) WITH (...)</c>, que falha em compatibilidades sem JSON.
/// </summary>
public sealed class ConsultasPromovidasPorAdmissoesSpec : Specification<Consulta>
{
  public ConsultasPromovidasPorAdmissoesSpec(IEnumerable<Guid> admissaoIds)
  {
    Guid[] idArray = admissaoIds.Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query.Where(c =>
      c.AdmissaoId != null
      && EF.Constant(idArray).Contains(c.AdmissaoId.Value)
      && c.DeletedOn == null);
  }
}
