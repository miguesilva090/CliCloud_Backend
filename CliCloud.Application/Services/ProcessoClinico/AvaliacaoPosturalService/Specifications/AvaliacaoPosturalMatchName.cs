using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.Specifications
{
    public class AvaliacaoPosturalMatchName : Specification<AvaliacaoPostural>
    {
        public AvaliacaoPosturalMatchName(Guid utenteId, DateTime data, TimeSpan hora)
        {
            Query.Where(x =>
                x.UtenteId == utenteId &&
                x.Data == data &&
                x.Hora == hora);
        }
    }
}
