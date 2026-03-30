using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using System.Globalization;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Specifications
{
    public class OdontogramaDefinitivoSearchTable : Specification<OdontogramaDefinitivo>
    {
        public OdontogramaDefinitivoSearchTable(
            string? keyword = "",
            Guid? utenteId = null,
            Guid? consultaId = null,
            string? dynamicOrder = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Descricao.Contains(keyword) ||
                    (x.CodigoSuperficie != null && x.CodigoSuperficie.Contains(keyword)) ||
                    x.NumeroDente.ToString(CultureInfo.InvariantCulture).Contains(keyword));
            }

            if (utenteId.HasValue)
            {
                _ = Query.Where(x => x.UtenteId == utenteId.Value);
            }

            if (consultaId.HasValue)
            {
                _ = Query.Where(x => x.ConsultaId == consultaId.Value);
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query
                    .OrderBy(x => x.NumeroDente)
                    .ThenBy(x => x.CodigoSuperficie);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}
