using Ardalis.Specification;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.MedicacaoCronicaService.Specifications
{
  public class MedicacaoCronicaByUtenteAndCnpem : Specification<MedicacaoCronica>
  {
    public MedicacaoCronicaByUtenteAndCnpem(Guid utenteId, string cnpem)
    {
      var key = cnpem.Trim();
      _ = Query.Where(x =>
        x.UtenteId == utenteId &&
        x.Cnpem == key &&
        x.DataFim == null);
    }
  }
}