using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService.Specifications
{
  public class SubsistemaServicoSearchTable : Specification<SubsistemaServico>
  {
    public SubsistemaServicoSearchTable(Guid? servicoId = null, Guid? organismoId = null, string? dynamicOrder = "")
    {
      if (servicoId.HasValue)
      {
        _ = Query.Where(x => x.ServicoId == servicoId.Value);
      }

      if (organismoId.HasValue)
      {
        _ = Query.Where(x => x.OrganismoId == organismoId.Value);
      }

      if (string.IsNullOrEmpty(dynamicOrder))
      {
        _ = Query.OrderBy(x => x.ServicoId).ThenBy(x => x.OrganismoId);
      }
      else
      {
        _ = Query.OrderBy(dynamicOrder);
      }
    }
  }
}
