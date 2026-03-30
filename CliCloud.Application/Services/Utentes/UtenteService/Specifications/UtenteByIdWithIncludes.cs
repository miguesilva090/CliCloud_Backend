using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
  public class UtenteByIdWithIncludes : Specification<Utente>
  {
    public UtenteByIdWithIncludes(Guid id)
    {
      _ = Query
        .Include(x => x.Rua)
        .Include(x => x.Freguesia)
        .Include(x => x.Concelho)
        .Include(x => x.Distrito)
        .Include(x => x.EstadoCivil)
        .Include(x => x.GrupoSanguineo)
        .Include(x => x.ProvenienciaUtente)
        .Include(x => x.Organismo)
        .Include(x => x.SeguradoraOrganismo)
        .Include(x => x.Empresa)
        .Include(x => x.CentroSaude)
        .Include(x => x.MedicoExterno)
        .Include(x => x.Medico)
        .Include(x => x.SubsistemaLinhas)
        .ThenInclude(s => s.Empresa)
        .Include(x => x.SubsistemaLinhas)
        .ThenInclude(s => s.Organismo)
        .Include(x => x.Habilitacao)
        .Include(x => x.Profissao)
        .Include(x => x.Sexo)
        .Where(x => x.Id == id);
    }
  }
}
