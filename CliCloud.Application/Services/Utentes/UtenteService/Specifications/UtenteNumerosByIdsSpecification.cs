using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
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
