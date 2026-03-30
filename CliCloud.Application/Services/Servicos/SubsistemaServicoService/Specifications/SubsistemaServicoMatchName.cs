using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService.Specifications
{
  public class SubsistemaServicoMatchName : Specification<SubsistemaServico>
  {
    public SubsistemaServicoMatchName(Guid servicoId, Guid organismoId, Guid subsistemaId)
    {
      _ = Query.Where(h =>
        h.ServicoId == servicoId &&
        h.OrganismoId == organismoId &&
        h.SubsistemaId == subsistemaId);
    }
  }
}
