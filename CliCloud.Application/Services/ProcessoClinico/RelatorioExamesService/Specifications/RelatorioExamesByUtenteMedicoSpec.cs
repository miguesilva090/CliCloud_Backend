using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.Specifications
{
    public class RelatorioExamesByUtenteMedicoSpec : Specification<RelatorioExames> 
    {
        public RelatorioExamesByUtenteMedicoSpec(Guid utenteId, Guid medicoId)
        {
            Query.Where(x => x.UtenteId == utenteId && x.MedicoId == medicoId);
        }
    }
}