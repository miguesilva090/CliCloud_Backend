using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Specifications
{
    public class OdontogramaDefinitivoByUtenteConsultaSpecification : Specification<OdontogramaDefinitivo>
    {
        public OdontogramaDefinitivoByUtenteConsultaSpecification(Guid utenteId, Guid consultaId)
        {
            _ = Query
                .Where(x => x.UtenteId == utenteId && x.ConsultaId == consultaId)
                .OrderBy(x => x.NumeroDente)
                .ThenBy(x => x.CodigoSuperficie);
        }
    }
}

