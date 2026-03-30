using Ardalis.Specification;
using CentroSaudeEntity = CliCloud.Domain.Entities.CentroSaude.CentroSaude;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.Specifications
{
    public class CentroSaudeMatchName : Specification<CentroSaudeEntity>
    {
        public CentroSaudeMatchName(string nome)
        {
            _ = Query.Where(x => x.Nome == nome);
        }
    }
}
