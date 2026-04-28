using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
  /// <summary>
  /// Usa <see cref="EF.Constant{T}(T)"/> no filtro para o SQL Server gerar <c>IN (...)</c> em vez de
  /// <c>OPENJSON(...) WITH (...)</c>, que falha em motores/compatibilidade sem JSON (erro junto a <c>WITH</c>).
  /// </summary>
  public class UtenteNumerosByIdsSpecification : Specification<Utente>
  {
    public UtenteNumerosByIdsSpecification(IEnumerable<Guid> ids)
    {
      Guid[] idArray = ids.Distinct().ToArray();
      if (idArray.Length == 0)
      {
        _ = Query.Where(_ => false);
        return;
      }

      _ = Query.Where(u => EF.Constant(idArray).Contains(u.Id));
    }
  }
}
