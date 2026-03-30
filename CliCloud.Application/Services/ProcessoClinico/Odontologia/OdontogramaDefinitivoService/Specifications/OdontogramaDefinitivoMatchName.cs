using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Specifications
{
    public class OdontogramaDefinitivoMatchLinha : Specification<OdontogramaDefinitivo>
    {
        public OdontogramaDefinitivoMatchLinha(Guid consultaId, int numeroDente, string? codigoSuperficie)
        {
            _ = Query.Where(h =>
                h.ConsultaId == consultaId &&
                h.NumeroDente == numeroDente &&
                h.CodigoSuperficie == codigoSuperficie);
        }
    }
}
