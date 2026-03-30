using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.Specifications
{
    public class AvaliacaoAntropometricaMatchName : Specification<AvaliacaoAntropometrica>
    {
        public AvaliacaoAntropometricaMatchName(Guid utenteId, DateTime data, TimeSpan hora)
        {
            Query.Where(x =>
                x.UtenteId == utenteId &&
                x.Data == data &&
                x.Hora == hora);
        }
    }
}
