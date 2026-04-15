using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Specifications
{
    public class FichaClinicaSecaoConteudoByUtenteAndSeparadorSpec : Specification<FichaClinicaSecaoConteudo>
    {
        public FichaClinicaSecaoConteudoByUtenteAndSeparadorSpec(Guid utenteId, Guid separadorId)
        {
            Query
                .Where( x => x.UtenteId == utenteId && x.Campo.SeparadorId == separadorId)
                .Include(x => x.Campo)
                .ThenInclude(c => c.Separador)
                .OrderBy(x => x.Campo.Ordem)
                .ThenBy(x => x.Campo.Nome);
        }
    }
}