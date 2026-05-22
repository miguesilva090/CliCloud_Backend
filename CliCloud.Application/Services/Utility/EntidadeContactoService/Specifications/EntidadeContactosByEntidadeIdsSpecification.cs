using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

public sealed class EntidadeContactosByEntidadeIdsSpecification : Specification<EntidadeContacto>
{
  public EntidadeContactosByEntidadeIdsSpecification(IEnumerable<Guid> entidadeIds)
  {
   Guid[] ids = entidadeIds.Where(x => x != Guid.Empty).Distinct().ToArray();
   if(ids.Length == 0)
   {
    _ = Query.Where(_ => false);
    return;
   }
   _ = Query.Where(x => EF.Constant(ids).Contains(x.EntidadeId));
  }
}
