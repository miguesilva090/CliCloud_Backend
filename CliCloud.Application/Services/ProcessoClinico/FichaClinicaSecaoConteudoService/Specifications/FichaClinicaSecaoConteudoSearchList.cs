using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;


namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Specifications
{
    public class FichaClinicaSecaoConteudoSearchList : Specification<FichaClinicaSecaoConteudo>
    {
        public FichaClinicaSecaoConteudoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Texto.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
