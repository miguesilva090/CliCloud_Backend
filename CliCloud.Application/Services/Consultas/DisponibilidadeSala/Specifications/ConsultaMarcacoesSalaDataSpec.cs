using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.DisponibilidadeSala.Specifications;

public sealed class ConsultaMarcacoesSalaDataSpec : Specification<ConsultaMarcacao>
{
  public ConsultaMarcacoesSalaDataSpec(Guid salaId, DateTime data)
  {
    _ = Query.Where(x =>
      x.DeletedOn == null
      && x.SalaId == salaId
      && x.Data.HasValue
      && x.Data.Value.Date == data.Date
    );
  }
}
