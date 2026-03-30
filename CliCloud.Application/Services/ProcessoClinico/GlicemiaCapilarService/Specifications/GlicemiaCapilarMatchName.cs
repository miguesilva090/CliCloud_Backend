using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.GlicemiaCapilarService.Specifications
{
    public class GlicemiaCapilarMatchName : Specification<GlicemiaCapilar>
    {
        public GlicemiaCapilarMatchName(Guid utenteId, DateTime data, TimeSpan hora)
        {
            Query.Where(x =>
                x.UtenteId == utenteId &&
                x.Data == data &&
                x.Hora == hora);
        }
    }
}
