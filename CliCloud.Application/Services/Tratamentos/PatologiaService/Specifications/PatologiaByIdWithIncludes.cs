using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.Specifications
{
  public class PatologiaByIdWithIncludes : Specification<Patologia>
  {
    public PatologiaByIdWithIncludes(Guid id)
    {
      _ = Query
        .Where(x => x.Id == id)
        .Include(x => x.Organismo)
        .Include(x => x.LocalTratamento)
        .Include(x => x.PatologiaServicos)
          .ThenInclude(ps => ps.SubsistemaServico)
            .ThenInclude(ss => ss.Servico)
        .Include(x => x.PatologiaDoencas);
    }
  }
}
