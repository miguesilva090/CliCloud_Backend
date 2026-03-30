using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.Specifications
{
  public class EvolucaoTratamentoFicheiroByEvolucaoId
    : Specification<EvolucaoTratamentoFicheiro>
  {
    public EvolucaoTratamentoFicheiroByEvolucaoId(Guid evolucaoTratamentoId)
    {
      _ = Query.Where(x => x.EvolucaoTratamentoId == evolucaoTratamentoId)
               .OrderByDescending(x => x.CreatedOn);
    }
  }
}

