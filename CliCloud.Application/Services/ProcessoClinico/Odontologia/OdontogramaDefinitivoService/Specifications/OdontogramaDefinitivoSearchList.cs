using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using System.Globalization;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Specifications
{
    public class OdontogramaDefinitivoSearchList : Specification<OdontogramaDefinitivo>
    {
        public OdontogramaDefinitivoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Descricao.Contains(keyword) ||
                    (x.CodigoSuperficie != null && x.CodigoSuperficie.Contains(keyword)) ||
                    x.NumeroDente.ToString(CultureInfo.InvariantCulture).Contains(keyword));
            }

            _ = Query
                .OrderBy(x => x.NumeroDente)
                .ThenBy(x => x.CodigoSuperficie);
        }
    }
}
