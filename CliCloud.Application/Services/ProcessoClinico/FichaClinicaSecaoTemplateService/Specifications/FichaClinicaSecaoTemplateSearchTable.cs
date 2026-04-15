using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Specifications
{
    public class FichaClinicaSecaoTemplateSearchTable : Specification<FichaClinicaSecaoTemplate>
    {
        public FichaClinicaSecaoTemplateSearchTable(
            Guid utilizadorId,
            string? keyword = "",
            string? dynamicOrder = ""
        )
        {
            _ = Query.Where(x => x.UtilizadorId == utilizadorId);

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Codigo.Contains(keyword) ||
                    x.Nome.Contains(keyword)
                );
            }


            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.Nome); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }


        }
    }
}
