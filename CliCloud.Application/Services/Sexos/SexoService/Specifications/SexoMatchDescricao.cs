using Ardalis.Specification;
using CliCloud.Domain.Entities.Sexos;

namespace CliCloud.Application.Services.Sexos.SexoService.Specifications
{
    public class SexoMatchDescricao : Specification<Sexo>
    {
        public SexoMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
