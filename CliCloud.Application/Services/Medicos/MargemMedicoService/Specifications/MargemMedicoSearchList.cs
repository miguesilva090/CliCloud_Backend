using Ardalis.Specification;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.Specifications
{
    public class MargemMedicoSearchList : Specification<MargemMedico>
    {
        public MargemMedicoSearchList(string? keyword = "")
        {
            _ = Query
                .Include(x => x.Servico)
                .Include(x => x.Medico);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    (x.Servico != null && x.Servico.Designacao.Contains(keyword)) ||
                    (x.Medico != null && x.Medico.Nome.Contains(keyword)));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
