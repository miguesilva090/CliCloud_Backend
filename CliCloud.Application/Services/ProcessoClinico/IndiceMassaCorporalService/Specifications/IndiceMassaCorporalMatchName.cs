using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.IndiceMassaCorporalService.Specifications
{
    public class IndiceMassaCorporalMatchName : Specification<IndiceMassaCorporal>
    {
        public IndiceMassaCorporalMatchName(Guid utenteId, DateTime data, TimeSpan hora)
        {
            Query.Where(x =>
                x.UtenteId == utenteId &&
                x.Data == data &&
                x.Hora == hora);
        }
    }
}
