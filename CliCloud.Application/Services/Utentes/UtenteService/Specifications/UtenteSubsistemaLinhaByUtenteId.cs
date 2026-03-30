using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
  public class UtenteSubsistemaLinhaByUtenteId : Specification<UtenteSubsistemaLinha>
  {
    public UtenteSubsistemaLinhaByUtenteId(Guid utenteId)
    {
      _ = Query.Where(x => x.UtenteId == utenteId);
    }
  }
}
