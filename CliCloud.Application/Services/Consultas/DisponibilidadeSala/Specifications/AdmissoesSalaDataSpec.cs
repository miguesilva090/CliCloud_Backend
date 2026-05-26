using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.DisponibilidadeSala.Specifications;

public sealed class AdmissoesSalaDataSpec : Specification<Admissao>
{
  public AdmissoesSalaDataSpec(Guid salaId, DateTime data)
  {
    _ = Query.Where(x =>
      x.DeletedOn == null
      && x.SalaId == salaId
      && x.Data.HasValue
      && x.Data.Value.Date == data.Date
    );
  }
}
