using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;


namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Specifications
{
    public class FichaClinicaSecaoTemplateSearchList : Specification<FichaClinicaSecaoTemplate>
    {
        public FichaClinicaSecaoTemplateSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.Codigo.Contains(keyword) ||
                    x.Nome.Contains(keyword)
                );
            }

            _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.Nome); // default sort order

        }
    }
}
