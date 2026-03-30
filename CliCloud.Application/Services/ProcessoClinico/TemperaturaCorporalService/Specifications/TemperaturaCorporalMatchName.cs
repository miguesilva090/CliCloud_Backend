using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.TemperaturaCorporalService.Specifications
{
    public class TemperaturaCorporalMatchName : Specification<TemperaturaCorporal>
    {
        public TemperaturaCorporalMatchName(Guid utenteId, DateTime data, TimeSpan hora)
        {
            Query.Where(x =>
                x.UtenteId == utenteId &&
                x.Data == data &&
                x.Hora == hora);
        }
    }
}
