using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.SalaService.Specifications
{
  public class SalaMatchNomeNumero : Specification<Sala>
  {
    public SalaMatchNomeNumero(string nome, int numeroSala, Guid? exceptId = null)
    {
      _ = Query.Where(x => x.Nome == nome || x.NumeroSala == numeroSala);

      if (exceptId.HasValue)
      {
        _ = Query.Where(x => x.Id != exceptId.Value);
      }
    }
  }
}
