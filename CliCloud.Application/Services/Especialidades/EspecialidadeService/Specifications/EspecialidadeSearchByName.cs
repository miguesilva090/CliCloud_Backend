using Ardalis.Specification;
using CliCloud.Domain.Entities.Especialidades;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.Specifications
{
    public class EspecialidadeSearchByName : Specification<Especialidade>
    {
        public EspecialidadeSearchByName(string nome)
        {
            _ = Query.Where(x => x.Nome.Contains(nome));
            _ = Query.Include(x => x.CategoriaEspecialidade);
        }
    }
}
