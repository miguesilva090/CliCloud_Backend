using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  public class ClinicaMatchNome : Specification<Clinica>
  {
    public ClinicaMatchNome(string nome) => _ = Query.Where(x => x.Nome == nome);
  }
}
