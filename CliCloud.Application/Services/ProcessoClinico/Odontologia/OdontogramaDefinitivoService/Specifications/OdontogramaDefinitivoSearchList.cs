using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

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
                    x.NumeroDente.ToString().Contains(keyword));
            }

            _ = Query
                .OrderBy(x => x.NumeroDente)
                .ThenBy(x => x.CodigoSuperficie);
        }
    }
}
