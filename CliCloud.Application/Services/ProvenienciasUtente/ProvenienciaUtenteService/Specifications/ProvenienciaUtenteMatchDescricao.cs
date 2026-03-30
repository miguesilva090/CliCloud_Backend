using Ardalis.Specification;
using CliCloud.Domain.Entities.ProvenienciasUtente;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Specifications
{
    public class ProvenienciaUtenteMatchDescricao : Specification<ProvenienciaUtente>
    {
        public ProvenienciaUtenteMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
