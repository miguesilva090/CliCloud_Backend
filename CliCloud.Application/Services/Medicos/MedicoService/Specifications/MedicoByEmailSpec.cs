using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MedicoService.Specifications
{
  public class MedicoByEmailSpec : Specification<Medico>
  {
    public MedicoByEmailSpec(string email)
    {
      string normalized = email.Trim().ToLowerInvariant();
      _ = Query.Where(m => m.Email != null && m.Email.ToLower() == normalized);
      _ = Query.OrderBy(m => m.Nome);
    }
  }
}
