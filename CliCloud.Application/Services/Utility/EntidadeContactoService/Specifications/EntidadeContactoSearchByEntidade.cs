using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications
{
  public class EntidadeContactoSearchByEntidade : Specification<EntidadeContacto>
  {
    public EntidadeContactoSearchByEntidade(Guid entidadeId)
    {
      _ = Query.Where(x => x.EntidadeId == entidadeId);
    }
  }
}