using Ardalis.Specification;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Application.Services.Tecnicos.TecnicoService.Specifications;

public sealed class TecnicoByEmailSpec : Specification<Tecnico>
{
  public TecnicoByEmailSpec(string email)
  {
    string normalized = email.Trim().ToLowerInvariant();
    _ = Query.Where(t => t.Email != null && t.Email.ToLower() == normalized);
    _ = Query.OrderBy(t => t.Nome);
  }
}
