using Ardalis.Specification;
using CliCloud.Domain.Entities.Alergias;


namespace CliCloud.Application.Services.AlergiasUtenteObsService.Specifications
{
    public class AlergiasUtenteObsSearchList : Specification<AlergiasUtenteObs>
    {
        public AlergiasUtenteObsSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    (x.Observacoes != null && x.Observacoes.Contains(keyword)) ||
                    (x.InformacaoImportante != null && x.InformacaoImportante.Contains(keyword)));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
