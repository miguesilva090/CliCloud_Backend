using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;


namespace CliCloud.Application.Services.Organismos.OrganismoService.Specifications
{
    public class OrganismoSearchList : Specification<Organismo>
    {
        public OrganismoSearchList(string? keyword = "", string? siglaFicheiro = null)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Nome.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(siglaFicheiro))
            {
                string normalized = siglaFicheiro
                    .Replace("/", "" , StringComparison.Ordinal)
                    .Replace("-", "", StringComparison.Ordinal)
                    .ToUpperInvariant();

                if(normalized == "SADGNR")
                {
                    _ = Query.Where(x => x.SADGNR);
                }
                else if ( normalized == "SADPSP")
                {
                    _ = Query.Where(x => x.SADPSP);
                }
                else if ( normalized == "ADM")
                {
                    _ = Query.Where(x => x.ADM);
                }
            }

            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}