using Ardalis.Specification;
using CliCloud.Domain.Entities.EstadosCivis;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Specifications
{
    public class EstadoCivilMatchDescricao : Specification<EstadoCivil>
    {
        public EstadoCivilMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
