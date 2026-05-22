using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MedicoService.Specifications;

public sealed class MedicoByLetraSpecification : Specification<Medico>
{
  public MedicoByLetraSpecification(string letra)
  {
    string key = letra.Trim();
    _ = Query.Where(m => m.Letra != null && m.Letra.Trim().ToLower() == key.ToLower());
  }
}
