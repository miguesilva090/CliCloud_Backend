using Ardalis.Specification;
using CliCloud.Domain.Entities.Especialidades;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Specifications
{
    public class CategoriaEspecialidadeMatchDescricao : Specification<CategoriaEspecialidade>
    {
        public CategoriaEspecialidadeMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
