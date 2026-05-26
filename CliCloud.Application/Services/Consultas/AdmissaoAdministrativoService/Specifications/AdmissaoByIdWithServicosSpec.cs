using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class AdmissaoByIdWithServicosSpec : Specification<Admissao>
{
  public AdmissaoByIdWithServicosSpec(Guid id)
  {
    _ = Query
      .Where(x => x.Id == id && x.DeletedOn == null)
      .Include(x => x.Utente)
      .Include(x => x.Medico)
      .Include(x => x.DoencaPrincipal)
      .Include(x => x.DoencaSecundaria)
      .Include(x => x.TipoAdmissao)
      .Include(x => x.Servicos);
  }
}
