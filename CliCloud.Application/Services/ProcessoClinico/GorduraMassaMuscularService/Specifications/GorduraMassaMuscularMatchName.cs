using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Specifications
{
    public class GorduraMassaMuscularMatchName : Specification<GorduraMassaMuscular>
    {
        public GorduraMassaMuscularMatchName(Guid utenteId, DateTime data, TimeSpan hora)
        {
            Query.Where(x =>
                x.UtenteId == utenteId &&
                x.Data == data &&
                x.Hora == hora);
        }
    }
}
