using Ardalis.Specification;
using CentroSaudeEntity = CliCloud.Domain.Entities.CentroSaude.CentroSaude;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.Specifications
{
    public class CentroSaudeSearchByName : Specification<CentroSaudeEntity>
    {
        public CentroSaudeSearchByName(string nome)
        {
            _ = Query.Where(x => x.Nome.Contains(nome));
        }
    }
}
