using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Specifications
{
    public class FichaClinicaSecaoConteudoMatchName : Specification<FichaClinicaSecaoConteudo>
    {
        public FichaClinicaSecaoConteudoMatchName(Guid utenteId, Guid campoId)
        {
            _ = Query.Where(h => h.UtenteId == utenteId && h.CampoId == campoId);
        }
    }
}
