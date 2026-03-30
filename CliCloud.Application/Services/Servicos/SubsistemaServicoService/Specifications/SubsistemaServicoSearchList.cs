using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService.Specifications
{
  public class SubsistemaServicoSearchList : Specification<SubsistemaServico>
  {
    public SubsistemaServicoSearchList(Guid? servicoId = null)
    {
      if (servicoId.HasValue)
      {
        _ = Query.Where(x => x.ServicoId == servicoId.Value);
      }

      _ = Query.OrderBy(x => x.ServicoId).ThenBy(x => x.OrganismoId);
    }
  }
}
