using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.AcordosService.Specifications
{
    public class AcordosSearchList : Specification<Acordos>
    {
        public AcordosSearchList(string? keyword = "")
        {
            _ = Query.Include(x => x.TipoExame).Include(x => x.Organismo);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    (x.CodigoSubsistema != null && x.CodigoSubsistema.Contains(keyword)) ||
                    (x.TipoExame != null && x.TipoExame.Designacao != null && x.TipoExame.Designacao.Contains(keyword)) ||
                    (x.Organismo != null && x.Organismo.Nome != null && x.Organismo.Nome.Contains(keyword)));
            }
            _ = Query.OrderBy(x => x.CodigoSubsistema);
        }
    }
}
