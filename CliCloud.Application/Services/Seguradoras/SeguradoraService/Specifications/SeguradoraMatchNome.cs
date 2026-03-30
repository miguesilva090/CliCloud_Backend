using Ardalis.Specification;
using CliCloud.Domain.Entities.Seguradoras;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.Specifications
{
  public class SeguradoraMatchNome : Specification<Seguradora>
  {
    public SeguradoraMatchNome(string nome) => _ = Query.Where(x => x.Nome == nome);
  }
}
