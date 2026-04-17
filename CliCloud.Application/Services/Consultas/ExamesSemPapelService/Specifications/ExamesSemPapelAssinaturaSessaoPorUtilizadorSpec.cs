using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService.Specifications;

public class ExamesSemPapelAssinaturaSessaoPorUtilizadorSpec : Specification<ExamesSemPapelAssinaturaSessao>
{
  public ExamesSemPapelAssinaturaSessaoPorUtilizadorSpec(Guid utilizadorId)
  {
    _ = Query.Where(x => x.UtilizadorId == utilizadorId);
  }
}
