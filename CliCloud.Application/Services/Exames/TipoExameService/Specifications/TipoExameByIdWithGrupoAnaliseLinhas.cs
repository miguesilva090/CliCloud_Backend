using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.TipoExameService.Specifications
{
    public class TipoExameByIdWithGrupoAnaliseLinhas : Specification<TipoExame>
    {
        public TipoExameByIdWithGrupoAnaliseLinhas(Guid id)
        {
            _ = Query.Where(x => x.Id == id)
                .Include(x => x.GrupoAnaliseLinhas)
                .ThenInclude(l => l.Analise);
        }
    }
}
